using SimpleProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimpleProject.Services
{
    public class LabelPreviewService
    {
        private readonly Label _label;
        private readonly int _scale = 12;
        private readonly int _dpi = 96;
        private double _pixelWidth;
        private double _pixelHeight;

        private RenderTargetBitmap _labelPreview;

        public LabelPreviewService(Label label) 
        {
            _label = label;
            CalculateLabelPreviewPixelSize();
            _labelPreview = CreateRenderTarget();
        }

        private RenderTargetBitmap CreateRenderTarget()
        {
            return new RenderTargetBitmap(
                (int)_pixelWidth,
                (int)_pixelHeight,
                _dpi,
                _dpi,
                PixelFormats.Pbgra32
            );
        }

        private DrawingVisual RenderLabelPart(bool renderDynamic)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                foreach (var element in _label.LabelElements)
                {
                    bool isDynamic = !string.IsNullOrEmpty(element.VariableName);
                    if (renderDynamic == isDynamic)
                    {
                        element.Draw(dc, _scale);
                    }
                }
            }
            return visual;
        }

        private DrawingVisual DrawDynamicLabelPart(string fieldTag)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                foreach (var element in _label.LabelElements)
                {
                    if (element.VariableName == fieldTag) 
                        element.Draw(dc, _scale);
                }
            }
            return visual;
        }

        private void CalculateLabelPreviewPixelSize()
        {
            double maxX = _label.LabelElements.Max(e => e.XEnd > 0 ? e.XEnd : e.X);
            double maxY = _label.LabelElements.Max(e => e.YEnd > 0 ? e.YEnd : e.Y);

            _pixelWidth = maxX * _scale;
            _pixelHeight = maxY * _scale;
        }

        public RenderTargetBitmap RenderDynamicLabelElements()
        {
            var dynamicVisual = RenderLabelPart(true);
            _labelPreview.Render(dynamicVisual);
            return _labelPreview;
        }
        
        public RenderTargetBitmap RenderDynamicLabelElement(string fieldTag)
        {
            var dynamicVisual = DrawDynamicLabelPart(fieldTag);
            _labelPreview.Render(dynamicVisual);
            return _labelPreview;
        }

        public RenderTargetBitmap RenderStaticLabelPreview()
        {
            var staticVisual = RenderLabelPart(false);
            _labelPreview.Render(staticVisual);
            return _labelPreview;
        }
    }
}
