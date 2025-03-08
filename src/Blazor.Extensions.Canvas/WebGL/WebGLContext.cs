using System;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Blazor.Extensions.Canvas.WebGL
{
    public class WebGLContext : RenderingContext
    {
        #region Properties
        public int DrawingBufferWidth { get; private set; }
        public int DrawingBufferHeight { get; private set; }
        #endregion

        public WebGLContext(BECanvasComponent reference, WebGLContextAttributes attributes = null)
            : base(reference, "WebGL", attributes)
        {
        }

        protected override async Task ExtendedInitializeAsync()
        {
            this.DrawingBufferWidth = await this.GetDrawingBufferWidthAsync();
            this.DrawingBufferHeight = await this.GetDrawingBufferHeightAsync();
        }

        #region Methods
        public async Task ClearColorAsync(float red, float green, float blue, float alpha) => await this.BatchCallAsync("clearColor", isMethodCall: true, red, green, blue, alpha);

        public async Task ClearAsync(BufferBits mask) => await this.BatchCallAsync("clear", isMethodCall: true, mask);

        private async Task<int> GetDrawingBufferWidthAsync() => await this.GetPropertyAsync<int>("drawingBufferWidth");

        private async Task<int> GetDrawingBufferHeightAsync() => await this.GetPropertyAsync<int>("drawingBufferHeight");

        public async Task<WebGLContextAttributes> GetContextAttributesAsync() => await this.CallMethodAsync<WebGLContextAttributes>("getContextAttributes");

        public async Task<bool> IsContextLostAsync() => await this.CallMethodAsync<bool>("isContextLost");

        public async Task ScissorAsync(int x, int y, int width, int height) => await this.BatchCallAsync("scissor", isMethodCall: true, x, y, width, height);

        public async Task ViewportAsync(int x, int y, int width, int height) => await this.BatchCallAsync("viewport", isMethodCall: true, x, y, width, height);

        public async Task ActiveTextureAsync(Texture texture) => await this.BatchCallAsync("activeTexture", isMethodCall: true, texture);

        public async Task BlendColorAsync(float red, float green, float blue, float alpha) => await this.BatchCallAsync("blendColor", isMethodCall: true, red, green, blue, alpha);

        public async Task BlendEquationAsync(BlendingEquation equation) => await this.BatchCallAsync("blendEquation", isMethodCall: true, equation);

        public async Task BlendEquationSeparateAsync(BlendingEquation modeRGB, BlendingEquation modeAlpha) => await this.BatchCallAsync("blendEquationSeparate", isMethodCall: true, modeRGB, modeAlpha);

        public async Task BlendFuncAsync(BlendingMode sfactor, BlendingMode dfactor) => await this.BatchCallAsync("blendFunc", isMethodCall: true, sfactor, dfactor);

        public async Task BlendFuncSeparateAsync(BlendingMode srcRGB, BlendingMode dstRGB, BlendingMode srcAlpha, BlendingMode dstAlpha) => await this.BatchCallAsync("blendFuncSeparate", isMethodCall: true, srcRGB, dstRGB, srcAlpha, dstAlpha);

        public async Task ClearDepthAsync(float depth) => await this.BatchCallAsync("clearDepth", isMethodCall: true, depth);

        public async Task ClearStencilAsync(int stencil) => await this.BatchCallAsync("clearStencil", isMethodCall: true, stencil);

        public async Task ColorMaskAsync(bool red, bool green, bool blue, bool alpha) => await this.BatchCallAsync("colorMask", isMethodCall: true, red, green, blue, alpha);

        public async Task CullFaceAsync(Face mode) => await this.BatchCallAsync("cullFace", isMethodCall: true, mode);

        public async Task DepthFuncAsync(CompareFunction func) => await this.BatchCallAsync("depthFunc", isMethodCall: true, func);

        public async Task DepthMaskAsync(bool flag) => await this.BatchCallAsync("depthMask", isMethodCall: true, flag);

        public async Task DepthRangeAsync(float zNear, float zFar) => await this.BatchCallAsync("depthRange", isMethodCall: true, zNear, zFar);

        public async Task DisableAsync(EnableCap cap) => await this.BatchCallAsync("disable", isMethodCall: true, cap);

        public async Task EnableAsync(EnableCap cap) => await this.BatchCallAsync("enable", isMethodCall: true, cap);

        public async Task FrontFaceAsync(FrontFaceDirection mode) => await this.BatchCallAsync("frontFace", isMethodCall: true, mode);

        public async Task<T> GetParameterAsync<T>(Parameter parameter) => await this.CallMethodAsync<T>("getParameter", parameter);

        public async Task<Error> GetErrorAsync() => await this.CallMethodAsync<Error>("getError");

        public async Task HintAsync(HintTarget target, HintMode mode) => await this.BatchCallAsync("hint", isMethodCall: true, target, mode);

        public async Task<bool> IsEnabledAsync(EnableCap cap) => await this.CallMethodAsync<bool>("isEnabled", cap);

        public async Task LineWidthAsync(float width) => await this.CallMethodAsync<object>("lineWidth", width);

        public async Task<object> PixelStoreIAsync(PixelStorageMode pname, int param) => await this.CallMethodAsync<object>("pixelStorei", pname, param);

        public async Task PolygonOffsetAsync(float factor, float units) => await this.BatchCallAsync("polygonOffset", isMethodCall: true, factor, units);

        public async Task SampleCoverageAsync(float value, bool invert) => await this.BatchCallAsync("sampleCoverage", isMethodCall: true, value, invert);

        public async Task StencilFuncAsync(CompareFunction func, int reference, uint mask) => await this.BatchCallAsync("stencilFunc", isMethodCall: true, func, reference, mask);

        public async Task StencilFuncSeparateAsync(Face face, CompareFunction func, int reference, uint mask) => await this.BatchCallAsync("stencilFuncSeparate", isMethodCall: true, face, func, reference, mask);

        public async Task StencilMaskAsync(uint mask) => await this.BatchCallAsync("stencilMask", isMethodCall: true, mask);

        public async Task StencilMaskSeparateAsync(Face face, uint mask) => await this.BatchCallAsync("stencilMaskSeparate", isMethodCall: true, face, mask);

        public async Task StencilOpAsync(StencilFunction fail, StencilFunction zfail, StencilFunction zpass) => await this.BatchCallAsync("stencilOp", isMethodCall: true, fail, zfail, zpass);

        public async Task StencilOpSeparateAsync(Face face, StencilFunction fail, StencilFunction zfail, StencilFunction zpass) => await this.BatchCallAsync("stencilOpSeparate", isMethodCall: true, face, fail, zfail, zpass);

        public async Task BindBufferAsync(BufferType target, WebGLBuffer buffer) => await this.BatchCallAsync("bindBuffer", isMethodCall: true, target, buffer);

        public async Task BufferDataAsync(BufferType target, int size, BufferUsageHint usage) => await this.BatchCallAsync("bufferData", isMethodCall: true, target, size, usage);

        public async Task BufferDataAsync<T>(BufferType target, T[] data, BufferUsageHint usage) where T : unmanaged => await this.BatchCallAsync("bufferData", true, target, MemoryMarshal.AsBytes(data.AsSpan()).ToArray(), usage);

        public async Task BufferSubDataAsync<T>(BufferType target, uint offset, T[] data) where T : unmanaged => await this.BatchCallAsync("bufferSubData", isMethodCall: true, target, offset, MemoryMarshal.AsBytes(data.AsSpan()).ToArray());

        public async Task<WebGLBuffer> CreateBufferAsync() => await this.CallMethodAsync<WebGLBuffer>("createBuffer");

        public async Task DeleteBufferAsync(WebGLBuffer buffer) => await this.BatchCallAsync("deleteBuffer", isMethodCall: true, buffer);

        public async Task<T> GetBufferParameterAsync<T>(BufferType target, BufferParameter pname) => await this.CallMethodAsync<T>("getBufferParameter", target, pname);

        public async Task<bool> IsBufferAsync(WebGLBuffer buffer) => await this.CallMethodAsync<bool>("isBuffer", buffer);

        public async Task BindFramebufferAsync(FramebufferType target, WebGLFramebuffer framebuffer) => await this.BatchCallAsync("bindFramebuffer", isMethodCall: true, target, framebuffer);

        public async Task<FramebufferStatus> CheckFramebufferStatusAsync(FramebufferType target) => await this.CallMethodAsync<FramebufferStatus>("checkFramebufferStatus", target);

        public async Task<WebGLFramebuffer> CreateFramebufferAsync() => await this.CallMethodAsync<WebGLFramebuffer>("createFramebuffer");

        public async Task DeleteFramebufferAsync(WebGLFramebuffer buffer) => await this.BatchCallAsync("deleteFramebuffer", isMethodCall: true, buffer);

        public async Task FramebufferRenderbufferAsync(FramebufferType target, FramebufferAttachment attachment, RenderbufferType renderbuffertarget, WebGLRenderbuffer renderbuffer) => await this.BatchCallAsync("framebufferRenderbuffer", isMethodCall: true, target, attachment, renderbuffertarget, renderbuffer);

        public async Task FramebufferTexture2DAsync(FramebufferType target, FramebufferAttachment attachment, Texture2DType textarget, WebGLTexture texture, int level) => await this.BatchCallAsync("framebufferTexture2D", isMethodCall: true, target, attachment, textarget, texture, level);

        public async Task<T> GetFramebufferAttachmentParameterAsync<T>(FramebufferType target, FramebufferAttachment attachment, FramebufferAttachmentParameter pname) => await this.CallMethodAsync<T>("getFramebufferAttachmentParameter", target, attachment, pname);

        public async Task<bool> IsFramebufferAsync(WebGLFramebuffer framebuffer) => await this.CallMethodAsync<bool>("isFramebuffer", framebuffer);

        public async Task ReadPixelsAsync(int x, int y, int width, int height, PixelFormat format, PixelType type, byte[] pixels) => await this.BatchCallAsync("readPixels", isMethodCall: true, x, y, width, height, format, type, pixels); //pixels should be an ArrayBufferView which the data gets read into

        public async Task BindRenderbufferAsync(RenderbufferType target, WebGLRenderbuffer renderbuffer) => await this.BatchCallAsync("bindRenderbuffer", isMethodCall: true, target, renderbuffer);

        public async Task<WebGLRenderbuffer> CreateRenderbufferAsync() => await this.CallMethodAsync<WebGLRenderbuffer>("createRenderbuffer");

        public async Task DeleteRenderbufferAsync(WebGLRenderbuffer buffer) => await this.BatchCallAsync("deleteRenderbuffer", isMethodCall: true, buffer);

        public async Task<T> GetRenderbufferParameterAsync<T>(RenderbufferType target, RenderbufferParameter pname) => await this.CallMethodAsync<T>("getRenderbufferParameter", target, pname);

        public async Task<bool> IsRenderbufferAsync(WebGLRenderbuffer renderbuffer) => await this.CallMethodAsync<bool>("isRenderbuffer", renderbuffer);

        public async Task RenderbufferStorageAsync(RenderbufferType type, RenderbufferFormat internalFormat, int width, int height) => await this.BatchCallAsync("renderbufferStorage", isMethodCall: true, type, internalFormat, width, height);

        public async Task BindTextureAsync(TextureType type, WebGLTexture texture) => await this.BatchCallAsync("bindTexture", isMethodCall: true, type, texture);

        public async Task CopyTexImage2DAsync(Texture2DType target, int level, PixelFormat format, int x, int y, int width, int height, int border) => await this.BatchCallAsync("copyTexImage2D", isMethodCall: true, target, level, format, x, y, width, height, border);

        public async Task CopyTexSubImage2DAsync(Texture2DType target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => await this.BatchCallAsync("copyTexSubImage2D", isMethodCall: true, target, level, xoffset, yoffset, x, y, width, height);

        public async Task CopyTexImage3DAsync(Texture3DType target, int level, PixelFormat format, int x, int y, int width, int height, int border) => await this.BatchCallAsync("copyTexImage3D", isMethodCall: true, target, level, format, x, y, width, height, border);

        public async Task CopyTexSubImage3DAsync(Texture3DType target, int level, int xoffset, int yoffset, int x, int y, int width, int height) => await this.BatchCallAsync("copyTexSubImage3D", isMethodCall: true, target, level, xoffset, yoffset, x, y, width, height);

        public async Task<WebGLTexture> CreateTextureAsync() => await this.CallMethodAsync<WebGLTexture>("createTexture");

        public async Task DeleteTextureAsync(WebGLTexture texture) => await this.BatchCallAsync("deleteTexture", isMethodCall: true, texture);

        public async Task GenerateMipmapAsync(TextureType target) => await this.BatchCallAsync("generateMipmap", isMethodCall: true, target);

        public async Task<T> GetTexParameterAsync<T>(TextureType target, TextureParameter pname) => await this.CallMethodAsync<T>("getTexParameter", target, pname);

        public async Task<bool> IsTextureAsync(WebGLTexture texture) => await this.CallMethodAsync<bool>("isTexture", texture);

        public async Task TexImage2DAsync<T>(Texture2DType target, int level, PixelFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, T[] pixels)
            where T : struct
            => await this.BatchCallAsync("texImage2D", isMethodCall: true, target, level, internalFormat, width, height, border, format, type, pixels);

        public async Task TexSubImage2DAsync<T>(Texture2DType target, int level, int xoffset, int yoffset, int width, int height, PixelFormat format, PixelType type, T[] pixels)
            where T : struct
            => await this.BatchCallAsync("texSubImage2D", isMethodCall: true, target, level, xoffset, yoffset, width, height, format, type, pixels);

        public async Task TexImage3DAsync<T>(Texture3DType target, int level, PixelFormat internalFormat, int width, int height, int depth, int border, PixelFormat format, PixelType type, T[] pixels)
            where T : struct
            => await this.BatchCallAsync("texImage3D", isMethodCall: true, target, level, internalFormat, width, height, depth, border, format, type, pixels);

        public async Task TexSubImage3DAsync<T>(Texture3DType target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, PixelFormat format, PixelType type, T[] pixels)
            where T : struct
            => await this.BatchCallAsync("texSubImage3D", isMethodCall: true, target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);

        public async Task TexStorage3DAsync(Texture3DType target, int level, PixelFormat internalformat, int width, int height, int depth) => await this.BatchCallAsync("texStorage3D", isMethodCall: true, target, level, internalformat, width, height, depth);

        public async Task TexParameterAsync(TextureType target, TextureParameter pname, float param) => await this.BatchCallAsync("texParameterf", isMethodCall: true, target, pname, param);

        public async Task TexParameterAsync(TextureType target, TextureParameter pname, int param) => await this.BatchCallAsync("texParameteri", isMethodCall: true, target, pname, param);

        public async Task AttachShaderAsync(WebGLProgram program, WebGLShader shader) => await this.BatchCallAsync("attachShader", isMethodCall: true, program, shader);

        public async Task BindAttribLocationAsync(WebGLProgram program, uint index, string name) => await this.BatchCallAsync("bindAttribLocation", isMethodCall: true, program, index, name);

        public async Task CompileShaderAsync(WebGLShader shader) => await this.BatchCallAsync("compileShader", isMethodCall: true, shader);

        public async Task<WebGLProgram> CreateProgramAsync() => await this.CallMethodAsync<WebGLProgram>("createProgram");

        public async Task<WebGLShader> CreateShaderAsync(ShaderType type) => await this.CallMethodAsync<WebGLShader>("createShader", type);

        public async Task DeleteProgramAsync(WebGLProgram program) => await this.BatchCallAsync("deleteProgram", isMethodCall: true, program);

        public async Task DeleteShaderAsync(WebGLShader shader) => await this.BatchCallAsync("deleteShader", isMethodCall: true, shader);

        public async Task DetachShaderAsync(WebGLProgram program, WebGLShader shader) => await this.BatchCallAsync("detachShader", isMethodCall: true, program, shader);

        public async Task<WebGLShader[]> GetAttachedShadersAsync(WebGLProgram program) => await this.CallMethodAsync<WebGLShader[]>("getAttachedShaders", program);

        public async Task<T> GetProgramParameterAsync<T>(WebGLProgram program, ProgramParameter pname) => await this.CallMethodAsync<T>("getProgramParameter", program, pname);

        public async Task<string> GetProgramInfoLogAsync(WebGLProgram program) => await this.CallMethodAsync<string>("getProgramInfoLog", program);

        public async Task<T> GetShaderParameterAsync<T>(WebGLShader shader, ShaderParameter pname) => await this.CallMethodAsync<T>("getShaderParameter", shader, pname);

        public async Task<WebGLShaderPrecisionFormat> GetShaderPrecisionFormatAsync(ShaderType shaderType, ShaderPrecision precisionType) => await this.CallMethodAsync<WebGLShaderPrecisionFormat>("getShaderPrecisionFormat", shaderType, precisionType);

        public async Task<string> GetShaderInfoLogAsync(WebGLShader shader) => await this.CallMethodAsync<string>("getShaderInfoLog", shader);

        public async Task<string> GetShaderSourceAsync(WebGLShader shader) => await this.CallMethodAsync<string>("getShaderSource", shader);

        public async Task<bool> IsProgramAsync(WebGLProgram program) => await this.CallMethodAsync<bool>("isProgram", program);

        public async Task<bool> IsShaderAsync(WebGLShader shader) => await this.CallMethodAsync<bool>("isShader", shader);

        public async Task LinkProgramAsync(WebGLProgram program) => await this.BatchCallAsync("linkProgram", isMethodCall: true, program);

        public async Task ShaderSourceAsync(WebGLShader shader, string source) => await this.BatchCallAsync("shaderSource", isMethodCall: true, shader, source);

        public async Task UseProgramAsync(WebGLProgram program) => await this.BatchCallAsync("useProgram", isMethodCall: true, program);

        public async Task ValidateProgramAsync(WebGLProgram program) => await this.BatchCallAsync("validateProgram", isMethodCall: true, program);

        public async Task DisableVertexAttribArrayAsync(uint index) => await this.BatchCallAsync("disableVertexAttribArray", isMethodCall: true, index);

        public async Task EnableVertexAttribArrayAsync(uint index) => await this.BatchCallAsync("enableVertexAttribArray", isMethodCall: true, index);

        public async Task<WebGLActiveInfo> GetActiveAttribAsync(WebGLProgram program, uint index) => await this.CallMethodAsync<WebGLActiveInfo>("getActiveAttrib", program, index);

        public async Task<WebGLActiveInfo> GetActiveUniformAsync(WebGLProgram program, uint index) => await this.CallMethodAsync<WebGLActiveInfo>("getActiveUniform", program, index);

        public async Task<int> GetAttribLocationAsync(WebGLProgram program, string name) => await this.CallMethodAsync<int>("getAttribLocation", program, name);

        public async Task<T> GetUniformAsync<T>(WebGLProgram program, WebGLUniformLocation location) => await this.CallMethodAsync<T>("getUniform", program, location);

        public async Task<WebGLUniformLocation> GetUniformLocationAsync(WebGLProgram program, string name) => await this.CallMethodAsync<WebGLUniformLocation>("getUniformLocation", program, name);

        public async Task<T> GetVertexAttribAsync<T>(uint index, VertexAttribute pname) => await this.CallMethodAsync<T>("getVertexAttrib", index, pname);

        public async Task<long> GetVertexAttribOffsetAsync(uint index, VertexAttributePointer pname) => await this.CallMethodAsync<long>("getVertexAttribOffset", index, pname);

        public async Task VertexAttribPointerAsync(uint index, int size, DataType type, bool normalized, int stride, long offset) => await this.BatchCallAsync("vertexAttribPointer", isMethodCall: true, index, size, type, normalized, stride, offset);

        public async Task UniformAsync(WebGLUniformLocation location, params float[] value)
        {
            switch (value.Length)
            {
                case 1:
                    await this.BatchCallAsync("uniform1fv", isMethodCall: true, location, value);
                    break;
                case 2:
                    await this.BatchCallAsync("uniform2fv", isMethodCall: true, location, value);
                    break;
                case 3:
                    await this.BatchCallAsync("uniform3fv", isMethodCall: true, location, value);
                    break;
                case 4:
                    await this.BatchCallAsync("uniform4fv", isMethodCall: true, location, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value.Length, "Value array is empty or too long");
            }
        }

        public async Task UniformAsync(WebGLUniformLocation location, params int[] value)
        {
            switch (value.Length)
            {
                case 1:
                    await this.BatchCallAsync("uniform1iv", isMethodCall: true, location, value);
                    break;
                case 2:
                    await this.BatchCallAsync("uniform2iv", isMethodCall: true, location, value);
                    break;
                case 3:
                    await this.BatchCallAsync("uniform3iv", isMethodCall: true, location, value);
                    break;
                case 4:
                    await this.BatchCallAsync("uniform4iv", isMethodCall: true, location, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value.Length, "Value array is empty or too long");
            }
        }

        public async Task UniformMatrixAsync(WebGLUniformLocation location, bool transpose, float[] value)
        {
            switch (value.Length)
            {
                case 2 * 2:
                    await this.BatchCallAsync("uniformMatrix2fv", isMethodCall: true, location, transpose, value);
                    break;
                case 3 * 3:
                    await this.BatchCallAsync("uniformMatrix3fv", isMethodCall: true, location, transpose, value);
                    break;
                case 4 * 4:
                    await this.BatchCallAsync("uniformMatrix4fv", isMethodCall: true, location, transpose, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value.Length, "Value array has incorrect size");
            }
        }

        public async Task VertexAttribAsync(uint index, params float[] value)
        {
            switch (value.Length)
            {
                case 1:
                    await this.BatchCallAsync("vertexAttrib1fv", isMethodCall: true, index, value);
                    break;
                case 2:
                    await this.BatchCallAsync("vertexAttrib2fv", isMethodCall: true, index, value);
                    break;
                case 3:
                    await this.BatchCallAsync("vertexAttrib3fv", isMethodCall: true, index, value);
                    break;
                case 4:
                    await this.BatchCallAsync("vertexAttrib4fv", isMethodCall: true, index, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value.Length, "Value array is empty or too long");
            }
        }

        public async Task DrawArraysAsync(Primitive mode, int first, int count) => await this.BatchCallAsync("drawArrays", isMethodCall: true, mode, first, count);

        public async Task DrawElementsAsync(Primitive mode, int count, DataType type, long offset) => await this.BatchCallAsync("drawElements", isMethodCall: true, mode, count, type, offset);

        public async Task FinishAsync() => await this.BatchCallAsync("finish", isMethodCall: true);

        public async Task FlushAsync() => await this.BatchCallAsync("flush", isMethodCall: true);

        #endregion
    }
}
