export class ContextManager {
  constructor(contextName) {
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

  add(canvas, parameters) {
    if (!canvas) throw new Error("Invalid canvas.");
    if (this.contexts.get(canvas.id)) return;

    let context;
    if (parameters)
      context = canvas.getContext(this.contextName, parameters);
    else
      context = canvas.getContext(this.contextName);

    if (!context) throw new Error("Invalid context.");

    this.contexts.set(canvas.id, context);
  }

  remove(canvas) {
    this.contexts.delete(canvas.id);
  }

  setProperty(canvas, property, value) {
    const context = this.getContext(canvas);
    this.setPropertyWithContext(context, property, value);
  }

  getProperty(canvas, property) {
    const context = this.getContext(canvas);
    return this.serialize(context[property]);
  }

  call(canvas, method, args) {
    const context = this.getContext(canvas);
    return this.callWithContext(context, method, args);
  }

  callBatch(canvas, batchedCalls) {
    const context = this.getContext(canvas);
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

  getContext(canvas) {
    if (!canvas) throw new Error("Invalid canvas.");

    const context = this.contexts.get(canvas.id);
    if (!context) throw new Error("Invalid context.");

    return context;
  }

  deserialize(method, object) {
    if (!this.webGLContext || object === undefined) return object;

    // If it's a serialized WebGL object, get the real instance.
    if (object && object.webGLType !== undefined && object.id !== undefined) {
      return this.webGLObjects.get(object.id);
    }

    // If it's an array and the method doesn’t expect a vector version, you might
    // still want to ensure it’s in the proper typed format.
    if (Array.isArray(object) && !method.endsWith("v")) {
      return new Uint8Array(object);
    }

    // For methods like bufferData/bufferSubData, you might have a fallback for Base64,
    // but with .NET 8 you can pass arrays directly.
    return object;
  }

  registerWebGLObject(obj) {
    if (!this.webGLObjectMap) {
      // Using a Map to store references for better memory management.
      this.webGLObjectMap = new Map();
      this.webGLObjectCounter = 0;
    }
    const id = this.webGLObjectCounter++;
    this.webGLObjectMap.set(id, obj);
    return id;
  }

  serialize(object) {
    // Special handling for TextMetrics.
    if (object instanceof TextMetrics) {
      return { width: object.width };
    }

    // If the object is an array or a typed array, return it directly.
    if (Array.isArray(object) || ArrayBuffer.isView(object)) {
      return object;
    }

    // For WebGL objects, register them and return a reference.
    if (this.webGLContext && object !== undefined) {
      const type = this.webGLTypes.find((type) => object instanceof type);
      if (type !== undefined) {
        // Register the object using a Map.
        const id = this.webGLObjectCounter++;
        this.webGLObjects.set(id, object);
        return {
          webGLType: type.name,
          id: id
        };
      }
    }

    // Otherwise, return the object as is.
    return object;
  }

  generateUUID() {
    // A lightweight UUID v4 generator using the Web Crypto API.
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, (c) =>
      (
        c ^
        crypto.getRandomValues(new Uint8Array(1))[0] &
        (15 >> (c / 4))
      ).toString(16)
    );
  }
}
