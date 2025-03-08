export class ContextManager {
  constructor(contextName) {
    this.canvas = null;
    this.contextName = contextName;
    this.contexts = new Map();
    this.webGLObjects = new Map();
    this.webGLObjectCounter = 0;
    this.patterns = new Map();
    this.webGLTypes = [
      WebGLBuffer,
      WebGLShader,
      WebGLProgram,
      WebGLFramebuffer,
      WebGLRenderbuffer,
      WebGLTexture,
      WebGLUniformLocation
    ];
    this.webGLContext = false;

    if (contextName === "2d") {
      this.prototypes = CanvasRenderingContext2D.prototype;
    } else if (contextName === "webgl2") {
      this.prototypes = WebGL2RenderingContext.prototype;
      this.webGLContext = true;
    } else {
      throw new Error(`Invalid context name: ${contextName}`);
    }
  }

  add(parameters) {
    if (!this.canvas) throw new Error("Invalid canvas.");
    if (this.contexts.get(this.canvas.id)) return;
    let context = parameters ? this.canvas.getContext(this.contextName, parameters) : this.canvas.getContext(this.contextName);
    if (!context) throw new Error("Invalid context.");
    this.contexts.set(this.canvas.id, context);
  }

  remove() {
    this.contexts.delete(this.canvas.id);
  }

  setProperty(property, value) {
    const context = this.getContext();
    this.setPropertyWithContext(context, property, value);
  }

  getProperty(property) {
    const context = this.getContext();
    return this.serialize(context[property]);
  }

  call(method, args) {
    const context = this.getContext();
    return this.callWithContext(context, method, args);
  }

  callBatch(batchedCalls) {
    const context = this.getContext();
    for (let i = 0; i < batchedCalls.length; i++) {
      let params = batchedCalls[i].slice(2);
      if (batchedCalls[i][1]) {
        this.callWithContext(context, batchedCalls[i][0], params);
      } else {
        this.setPropertyWithContext(
          context,
          batchedCalls[i][0],
          Array.isArray(params) && params.length > 0 ? params[0] : null
        );
      }
    }
  }

  callWithContext(context, method, args) {
    const result = this.prototypes[method].apply(
      context,
      args !== undefined ? args.map((value) => this.deserialize(method, value)) : []
    );

    if (method === "createPattern") {
      const key = this.generateUUID();
      this.patterns.set(key, result);
      return key;
    }
    return this.serialize(result);
  }

  setPropertyWithContext(context, property, value) {
    if (property === "fillStyle") {
      value = this.patterns.get(value) || value;
    }
    context[property] = this.deserialize(property, value);
  }

  getContext() {
    if (!this.canvas) throw new Error("Invalid canvas.");
    const context = this.contexts.get(this.canvas.id);
    if (!context) throw new Error("Invalid context.");
    return context;
  }

  deserialize(method, object) {
    if (!this.webGLContext || object === undefined) return object;
    if (object && object.webGLType !== undefined && object.id !== undefined) {
      return this.webGLObjects.get(object.id);
    }
    if (Array.isArray(object) && !method.endsWith("v")) {
      return new Uint8Array(object);
    }
    return object;
  }

  serialize(object) {
    if (object instanceof TextMetrics) {
      return { width: object.width };
    }
    if (Array.isArray(object) || ArrayBuffer.isView(object)) {
      return object;
    }
    if (this.webGLContext && object !== undefined) {
      const type = this.webGLTypes.find((type) => object instanceof type);
      if (type !== undefined) {
        const id = this.webGLObjectCounter++;
        this.webGLObjects.set(id, object);
        return { webGLType: type.name, id: id };
      }
    }
    return object;
  }

  resetWebGL() {
    let devicePixelRatio = window.devicePixelRatio || 1;
    let w = Math.floor(this.canvas.clientWidth * devicePixelRatio);
    let h = Math.floor(this.canvas.clientHeight * devicePixelRatio);
    if (this.canvas.width !== w || this.canvas.height !== h) {
      this.canvas.width = w;
      this.canvas.height = h;
    }
  }

  generateUUID() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
      (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & (15 >> (c / 4))).toString(16)
    );
  }
}

window["BlazorExtensions"] = {
  Canvas2d: new ContextManager("2d"),
  WebGL: new ContextManager("webgl2"),
  _dotNetInstance: null,
  _canvas: null,
  _resizeObserver: null,
  _resizeTimeout: null,

  initialize(dotNetInstance, canvas) {
    this._dotNetInstance = dotNetInstance;
    this._canvas = canvas;

    this.Canvas2d.canvas = canvas;
    this.WebGL.canvas = canvas;

    if (this._resizeObserver) {
      this._resizeObserver.disconnect()
    }

    this._resizeObserver = new ResizeObserver(() => this.debouncedResizeCanvas());
    this._resizeObserver.observe(this._canvas);
  },

  focusCanvas() {
    this._canvas.focus();
  },

  takeScreenshot() {
    this._canvas.toBlob(blob => {
      let reader = new FileReader();
      reader.readAsArrayBuffer(blob);
      reader.onloadend = () => {
        let bytes = new Uint8Array(reader.result);
        const url = URL.createObjectURL(blob);
        if (this._dotNetInstance) {
          this._dotNetInstance.invokeMethodAsync('ReceiveScreenshot', url, bytes);
        }
      };
    });
  },

  setCursor(value) {
    this._canvas.style.cursor = value;
  },

  getBoundingClientRect() {
    return this._canvas.getBoundingClientRect();
  },

  getCanvasSize() {
    let devicePixelRatio = window.devicePixelRatio || 1;
    let w = Math.floor(this._canvas.clientWidth * devicePixelRatio);
    let h = Math.floor(this._canvas.clientHeight * devicePixelRatio);
    return {
      width: w,
      height: h
    };
  },

  debouncedResizeCanvas() {
    clearTimeout(this._resizeTimeout);

    this._resizeTimeout = setTimeout(() => {
      this.resizeCanvas();
    }, 250);
  },

  resizeCanvas() {
    if (!this._canvas || !this._dotNetInstance) return;

    let devicePixelRatio = window.devicePixelRatio || 1;
    let w = Math.floor(this._canvas.clientWidth * devicePixelRatio);
    let h = Math.floor(this._canvas.clientHeight * devicePixelRatio);

    this._dotNetInstance.invokeMethodAsync("ResizeCanvas", w, h);
  }
};
