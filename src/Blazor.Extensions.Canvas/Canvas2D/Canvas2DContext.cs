using Blazor.Extensions.Canvas.Model;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Extensions.Canvas.Canvas2D
{
    public class Canvas2DContext : RenderingContext
    {
        #region Constants

        private readonly string[] _repeatNames = new[]
        {
            "repeat", "repeat-x", "repeat-y", "no-repeat"
        };

        #endregion

        #region Properties

        public object FillStyle { get; private set; } = "#000";

        public string StrokeStyle { get; private set; } = "#000";

        public string Font { get; private set; } = "10px sans-serif";

        public TextAlign TextAlign { get; private set; }

        public TextDirection Direction { get; private set; }

        public TextBaseline TextBaseline { get; private set; }

        public float LineWidth { get; private set; } = 1.0f;

        public LineCap LineCap { get; private set; }

        public LineJoin LineJoin { get; private set; }

        public float MiterLimit { get; private set; } = 10;

        public float LineDashOffset { get; private set; }

        public float ShadowBlur { get; private set; }

        public string ShadowColor { get; private set; } = "black";

        public float ShadowOffsetX { get; private set; }

        public float ShadowOffsetY { get; private set; }

        public float GlobalAlpha { get; private set; } = 1.0f;

        public string GlobalCompositeOperation { get; private set; } = "source-over";

        #endregion Properties

        public Canvas2DContext(BECanvasComponent reference)
            : base(reference, "Canvas2d")
        {
        }

        #region Property Setters

        public async Task SetFillStyleAsync(object value)
        {
            this.FillStyle = value;
            await this.BatchCallAsync("fillStyle", false, value);
        }

        public async Task SetStrokeStyleAsync(string value)
        {
            this.StrokeStyle = value;
            await this.BatchCallAsync("strokeStyle", isMethodCall: false, value);
        }

        public async Task SetFontAsync(string value)
        {
            this.Font = value;
            await this.BatchCallAsync("font", isMethodCall: false, value);
        }

        public async Task SetTextAlignAsync(TextAlign value)
        {
            this.TextAlign = value;
            await this.BatchCallAsync("textAlign", isMethodCall: false, value.ToString().ToLowerInvariant());
        }

        public async Task SetDirectionAsync(TextDirection value)
        {
            this.Direction = value;
            await this.BatchCallAsync("direction", isMethodCall: false, value.ToString().ToLowerInvariant());
        }

        public async Task SetTextBaselineAsync(TextBaseline value)
        {
            this.TextBaseline = value;
            await this.BatchCallAsync("textBaseline", isMethodCall: false, value.ToString().ToLowerInvariant());
        }

        public async Task SetLineWidthAsync(float value)
        {
            this.LineWidth = value;
            await this.BatchCallAsync("lineWidth", isMethodCall: false, value);
        }

        public async Task SetLineCapAsync(LineCap value)
        {
            this.LineCap = value;
            await this.BatchCallAsync("lineCap", isMethodCall: false, value.ToString().ToLowerInvariant());
        }

        public async Task SetLineJoinAsync(LineJoin value)
        {
            this.LineJoin = value;
            await this.BatchCallAsync("lineJoin", isMethodCall: false, value.ToString().ToLowerInvariant());
        }

        public async Task SetMiterLimitAsync(float value)
        {
            this.MiterLimit = value;
            await this.BatchCallAsync("miterLimit", isMethodCall: false, value.ToString().ToLowerInvariant());
        }

        public async Task SetLineDashOffsetAsync(float value)
        {
            this.LineDashOffset = value;
            await this.BatchCallAsync("lineDashOffset", isMethodCall: false, value);
        }

        public async Task SetShadowBlurAsync(float value)
        {
            this.ShadowBlur = value;
            await this.BatchCallAsync("shadowBlur", isMethodCall: false, value);
        }

        public async Task SetShadowColorAsync(string value)
        {
            this.ShadowColor = value;
            await this.BatchCallAsync("shadowColor", isMethodCall: false, value);
        }

        public async Task SetShadowOffsetXAsync(float value)
        {
            this.ShadowOffsetX = value;
            await this.BatchCallAsync("shadowOffsetX", isMethodCall: false, value);
        }

        public async Task SetShadowOffsetYAsync(float value)
        {
            this.ShadowOffsetY = value;
            await this.BatchCallAsync("shadowOffsetY", isMethodCall: false, value);
        }

        public async Task SetGlobalAlphaAsync(float value)
        {
            this.GlobalAlpha = value;
            await this.BatchCallAsync("globalAlpha", isMethodCall: false, value);
        }

        public async Task SetGlobalCompositeOperationAsync(string value)
        {
            this.GlobalCompositeOperation = value;
            await this.BatchCallAsync("globalCompositeOperation", isMethodCall: false, value);
        }

        #endregion Property Setters

        #region Methods

        public async Task FillRectAsync(double x, double y, double width, double height) => await this.BatchCallAsync("fillRect", isMethodCall: true, x, y, width, height);

        public async Task ClearRectAsync(double x, double y, double width, double height) => await this.BatchCallAsync("clearRect", isMethodCall: true, x, y, width, height);

        public async Task StrokeRectAsync(double x, double y, double width, double height) => await this.BatchCallAsync("strokeRect", isMethodCall: true, x, y, width, height);

        public async Task FillTextAsync(string text, double x, double y, double? maxWidth = null) => await this.BatchCallAsync("fillText", isMethodCall: true, maxWidth.HasValue ? new object[] { text, x, y, maxWidth.Value } : new object[] { text, x, y });

        public async Task StrokeTextAsync(string text, double x, double y, double? maxWidth = null) => await this.BatchCallAsync("strokeText", isMethodCall: true, maxWidth.HasValue ? new object[] { text, x, y, maxWidth.Value } : new object[] { text, x, y });

        public async Task<TextMetrics> MeasureTextAsync(string text) => await this.CallMethodAsync<TextMetrics>("measureText", text);

        public async Task<float[]> GetLineDashAsync() => await this.CallMethodAsync<float[]>("getLineDash");

        public async Task SetLineDashAsync(float[] segments) => await this.BatchCallAsync("setLineDash", isMethodCall: true, segments);

        public async Task BeginPathAsync() => await this.BatchCallAsync("beginPath", isMethodCall: true);

        public async Task ClosePathAsync() => await this.BatchCallAsync("closePath", isMethodCall: true);

        public async Task MoveToAsync(double x, double y) => await this.BatchCallAsync("moveTo", isMethodCall: true, x, y);

        public async Task LineToAsync(double x, double y) => await this.BatchCallAsync("lineTo", isMethodCall: true, x, y);

        public async Task BezierCurveToAsync(double cp1X, double cp1Y, double cp2X, double cp2Y, double x, double y) => await this.BatchCallAsync("bezierCurveTo", isMethodCall: true, cp1X, cp1Y, cp2X, cp2Y, x, y);

        public async Task QuadraticCurveToAsync(double cpx, double cpy, double x, double y) => await this.BatchCallAsync("quadraticCurveTo", isMethodCall: true, cpx, cpy, x, y);

        public async Task ArcAsync(double x, double y, double radius, double startAngle, double endAngle, bool? anticlockwise = null) => await this.BatchCallAsync("arc", isMethodCall: true, anticlockwise.HasValue ? new object[] { x, y, radius, startAngle, endAngle, anticlockwise.Value } : new object[] { x, y, radius, startAngle, endAngle });

        public async Task ArcToAsync(double x1, double y1, double x2, double y2, double radius) => await this.BatchCallAsync("arcTo", isMethodCall: true, x1, y1, x2, y2, radius);

        public async Task RectAsync(double x, double y, double width, double height) => await this.BatchCallAsync("rect", isMethodCall: true, x, y, width, height);

        public async Task FillAsync() => await this.BatchCallAsync("fill", isMethodCall: true);

        public async Task StrokeAsync() => await this.BatchCallAsync("stroke", isMethodCall: true);

        public async Task DrawFocusIfNeededAsync(ElementReference elementReference) => await this.BatchCallAsync("drawFocusIfNeeded", isMethodCall: true, elementReference);

        public async Task ScrollPathIntoViewAsync() => await this.BatchCallAsync("scrollPathIntoView", isMethodCall: true);

        public async Task ClipAsync() => await this.BatchCallAsync("clip", isMethodCall: true);

        public async Task<bool> IsPointInPathAsync(double x, double y) => await this.CallMethodAsync<bool>("isPointInPath", x, y);

        public async Task<bool> IsPointInStrokeAsync(double x, double y) => await this.CallMethodAsync<bool>("isPointInStroke", x, y);

        public async Task RotateAsync(float angle) => await this.BatchCallAsync("rotate", isMethodCall: true, angle);

        public async Task ScaleAsync(double x, double y) => await this.BatchCallAsync("scale", isMethodCall: true, x, y);

        public async Task TranslateAsync(double x, double y) => await this.BatchCallAsync("translate", isMethodCall: true, x, y);

        public async Task TransformAsync(double m11, double m12, double m21, double m22, double dx, double dy) => await this.BatchCallAsync("transform", isMethodCall: true, m11, m12, m21, m22, dx, dy);

        public async Task SetTransformAsync(double m11, double m12, double m21, double m22, double dx, double dy) => await this.BatchCallAsync("setTransform", isMethodCall: true, m11, m12, m21, m22, dx, dy);

        public async Task SaveAsync() => await this.BatchCallAsync("save", isMethodCall: true);

        public async Task RestoreAsync() => await this.BatchCallAsync("restore", isMethodCall: true);

        public async Task DrawImageAsync(ElementReference elementReference, double dx, double dy) => await this.BatchCallAsync("drawImage", isMethodCall: true, elementReference, dx, dy);

        public async Task DrawImageAsync(ElementReference elementReference, double dx, double dy, double dWidth, double dHeight) => await this.BatchCallAsync("drawImage", isMethodCall: true, elementReference, dx, dy, dWidth, dHeight);

        public async Task DrawImageAsync(ElementReference elementReference, double sx, double sy, double sWidth, double sHeight, double dx, double dy, double dWidth, double dHeight) => await this.BatchCallAsync("drawImage", isMethodCall: true, elementReference, sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight);

        public async Task<object> CreatePatternAsync(ElementReference image, RepeatPattern repeat) => await this.CallMethodAsync<object>("createPattern", image, this._repeatNames[(int)repeat]);

        #endregion Methods
    }
}
