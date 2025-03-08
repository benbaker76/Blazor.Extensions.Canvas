using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Extensions
{
    public abstract class RenderingContext : IDisposable
    {
        private readonly List<object[]> _batchedCallObjects = new();
        private readonly string _contextName;
        private readonly IJSRuntime _jsRuntime;
        private readonly object _parameters;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly DotNetObjectReference<RenderingContext> _dotNetInstance;

        private bool _awaitingBatchedCall;
        private bool _batching;
        private bool _initialized;
        public ElementReference Canvas { get; }

        public event Func<Size, Task> CanvasResized;
        public event Func<ScreenshotEventArgs, Task> ScreenshotReceived;

        internal RenderingContext(BECanvasComponent reference, string contextName, object parameters = null)
        {
            this._dotNetInstance = DotNetObjectReference.Create(this);
            this.Canvas = reference.CanvasReference;
            this._jsRuntime = reference.JSRuntime;
            this._contextName = contextName;
            this._parameters = parameters;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously; Reason: extension point for subclasses, which may do asynchronous work
        protected virtual async Task ExtendedInitializeAsync() { }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        internal async Task<RenderingContext> InitializeAsync()
        {
            await this._semaphoreSlim.WaitAsync();
            await this._jsRuntime.InvokeVoidAsync("BlazorExtensions.initialize", this._dotNetInstance, this.Canvas);

            if (!this._initialized)
            {
                await this._jsRuntime.InvokeVoidAsync($"BlazorExtensions.{this._contextName}.add", this._parameters);
                await this.ExtendedInitializeAsync();
                this._initialized = true;
            }
            this._semaphoreSlim.Release();
            return this;
        }

        protected async Task<T> GetPropertyAsync<T>(string property)
        {
            return await this._jsRuntime.InvokeAsync<T>($"BlazorExtensions.{this._contextName}.getProperty", property);
        }

        protected async Task<T> CallMethodAsync<T>(string method)
        {
            return await this._jsRuntime.InvokeAsync<T>($"BlazorExtensions.{this._contextName}.call", method);
        }

        protected async Task<T> CallMethodAsync<T>(string method, params object[] value)
        {
            return await this._jsRuntime.InvokeAsync<T>($"BlazorExtensions.{this._contextName}.call", method, value);
        }

        public async Task BeginBatchAsync()
        {
            await this._semaphoreSlim.WaitAsync();
            this._batching = true;
            this._semaphoreSlim.Release();
        }

        public async Task EndBatchAsync()
        {
            await this._semaphoreSlim.WaitAsync();

            await this.BatchCallInnerAsync();
        }

        protected async Task BatchCallAsync(string name, bool isMethodCall, params object[] value)
        {
            await this._semaphoreSlim.WaitAsync();

            var callObject = new object[value.Length + 2];
            callObject[0] = name;
            callObject[1] = isMethodCall;
            Array.Copy(value, 0, callObject, 2, value.Length);
            this._batchedCallObjects.Add(callObject);

            if (this._batching || this._awaitingBatchedCall)
            {
                this._semaphoreSlim.Release();
            }
            else
            {
                await this.BatchCallInnerAsync();
            }
        }

        public async Task TakeScreenshotAsync()
        {
            await this._jsRuntime.InvokeVoidAsync("BlazorExtensions.takeScreenshot");
        }

        public async Task FocusAsync()
        {
            await this._jsRuntime.InvokeVoidAsync("BlazorExtensions.focusCanvas");
        }

        public async Task<Rectangle> GetBoundingClientRectAsync()
        {
            try
            {
                return await this._jsRuntime.InvokeAsync<Rectangle>("BlazorExtensions.getBoundingClientRect");
            }
            catch (JSException)
            {
                return Rectangle.Empty;
            }
        }

        public async Task<Size> GetCanvasSizeAsync()
        {
            return await this._jsRuntime.InvokeAsync<Size>("BlazorExtensions.getCanvasSize");
        }

        public async Task SetCursorAsync(string cursor)
        {
            await this._jsRuntime.InvokeVoidAsync("BlazorExtensions.setCursor", cursor);
        }

        public async Task ResetWebGL()
        {
            await this._jsRuntime.InvokeVoidAsync($"BlazorExtensions.{this._contextName}.resetWebGL");
            await this.ExtendedInitializeAsync();
        }

        private async Task BatchCallInnerAsync()
        {
            this._awaitingBatchedCall = true;
            var currentBatch = this._batchedCallObjects.ToArray();
            this._batchedCallObjects.Clear();
            this._semaphoreSlim.Release();

            _ = await this._jsRuntime.InvokeAsync<object>($"BlazorExtensions.{this._contextName}.callBatch", (object)currentBatch);

            await this._semaphoreSlim.WaitAsync();
            this._awaitingBatchedCall = false;
            this._batching = false;
            this._semaphoreSlim.Release();
        }

        [JSInvokable]
        public async Task ResizeCanvas()
        {
            if (this.CanvasResized != null)
            {
                var size = await this._jsRuntime.InvokeAsync<Size>("BlazorExtensions.getCanvasSize");
                await this.CanvasResized(new Size(Math.Max(size.Width, 1), Math.Max(size.Height, 1)));
            }
        }

        [JSInvokable]
        public async Task ReceiveScreenshot(string url, byte[] bytes)
        {
            if (this.ScreenshotReceived != null)
            {
                await this.ScreenshotReceived(new ScreenshotEventArgs(url, bytes));
            }
        }

        public void Dispose()
        {
            Task.Run(async () => await this._jsRuntime.InvokeVoidAsync($"BlazorExtensions.{this._contextName}.remove"));
            GC.SuppressFinalize(this);
        }
    }

    public class ScreenshotEventArgs
    {
        public string Url { get; set; }
        public byte[] Bytes { get; set; }

        public ScreenshotEventArgs(string url, byte[] bytes)
        {
            this.Url = url;
            this.Bytes = bytes;
        }
    }
}
