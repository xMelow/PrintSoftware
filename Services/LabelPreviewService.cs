using SimpleProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimpleProject.Services
{
    public class LabelPreviewService
    {
        private RenderTargetBitmap _labelPreview;
        private readonly Label _label;
        private int _Scale = 12;
        private int _Dpi = 96;
        private double PixelWidth;
        private double PixelHeight;

        public LabelPreviewService(Label label) 
        {
            _label = label;
            CalculateLabelPreviewPixelSize();

            _labelPreview = new RenderTargetBitmap(
                            (int)PixelWidth,
                            (int)PixelHeight,
                            _Dpi,
                            _Dpi,
                            PixelFormats.Pbgra32);
        }

        private DrawingVisual RenderLabelPart(double scale, bool renderDynamic)
        {
            DrawingVisual visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                foreach (var element in _label.LabelElements)
                {
                    bool isDynamic = !string.IsNullOrEmpty(element.VariableName);
                    if (renderDynamic == isDynamic)
                    {
                        element.Draw(dc, scale);
                    }
                }
            }
            return visual;
        }

        private void CalculateLabelPreviewPixelSize()
        {
            double maxX = _label.LabelElements.Max(e => e.Xend > 0 ? e.Xend : e.X);
            double maxY = _label.LabelElements.Max(e => e.Yend > 0 ? e.Yend : e.Y);

            PixelWidth = maxX * _Scale;
            PixelHeight = maxY * _Scale;
        }

        public BitmapSource RenderDynamicLabelPreview()
        {
            DrawingVisual _dynamicVisual = RenderLabelPart(_Scale, renderDynamic: true);
            _labelPreview.Render(_dynamicVisual);
            return _labelPreview;
        }

        public BitmapSource RenderStaticLabelPreview()
        {
            DrawingVisual _dynamicVisual = RenderLabelPart(_Scale, renderDynamic: false);
            _labelPreview.Render(_dynamicVisual);
            return _labelPreview;
        }

        public BitmapSource RenderFullLabelPreview()
        {
            RenderDynamicLabelPreview();
            RenderStaticLabelPreview();
            return _labelPreview;
        }
    }
}
