import { ContextManager } from "./CanvasContextManager.js";

const blazorExtensions = "BlazorExtensions";

// Define the extension object with the two contexts.
const extensionObject = {
  Canvas2d: new ContextManager("2d"),
  WebGL: new ContextManager("webgl2")
};

function initialize() {
  if (typeof window !== "undefined" && !window[blazorExtensions]) {
    // When loaded in a browser via a <script> element,
    // expose the APIs on the global scope.
    window[blazorExtensions] = { ...extensionObject };
  } else {
    window[blazorExtensions] = { ...window[blazorExtensions], ...extensionObject };
  }
}

initialize();
