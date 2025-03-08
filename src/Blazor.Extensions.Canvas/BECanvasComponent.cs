using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Drawing;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blazor.Extensions
{
    public class BECanvasComponent : ComponentBase
    {
        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public long Width { get; set; }

        [Parameter]
        public long Height { get; set; }

        [Parameter]
        public string OnContextMenu { get; set; }

        protected readonly string Id = Guid.NewGuid().ToString();
        protected ElementReference _canvasRef;

        public ElementReference CanvasReference => this._canvasRef;

        [Inject]
        internal IJSRuntime JSRuntime { get; set; }

        public Task SetCanvasSizeAsync(long width, long height) =>
            Task.Run(() =>
            {
                this.Width = width;
                this.Height = height;
            });
    }
}
