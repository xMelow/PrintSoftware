using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PrintSoftware.Domain;
using PrintSoftware.Domain.Label;
using PrintSoftware.Domain.Label.LabelElements;
using PrintSoftware.Interfaces;

namespace PrintSoftware.Services
{
    public class LabelPreviewService
    {
        private readonly int _printerDPI = 300;
        private int _labelWidthInDots;
        private int _labelHeightInDots;
        private RenderTargetBitmap _labelPreview;
        private Label _label;

        public RenderTargetBitmap CreateLabelPreview(Label label)
        {
            _label = label;
            _labelPreview = CreateRenderTarget();
            return _labelPreview;
        }
        
        private RenderTargetBitmap CreateRenderTarget()
        {
            CalculateLabelPreviewPixels();
            
            // int maxX = _label.LabelElements.Max(e => e.X + (e is BoxElement be ? _label.Width : 0));
            // int maxY = _label.LabelElements.Max(e => e.Y + (e is BoxElement be ? _label.Height : 0));
            
            // double maxX = _label.LabelElements.Max(e => e.X);
            // double maxY = _label.LabelElements.Max(e => e.Y);

            
            return new RenderTargetBitmap(
                _labelWidthInDots,
                _labelHeightInDots,
                96,
                96,
                PixelFormats.Pbgra32
            );
        }
        private void CalculateLabelPreviewPixels()
        {
            _labelWidthInDots = (int)Math.Round((_label.Width * _printerDPI) / 25.4);
            _labelHeightInDots = (int)Math.Round((_label.Height * _printerDPI) / 25.4);
            
            // Console.WriteLine($"Preview size: {_labelWidthInDots}x{_labelHeightInDots}dots at {_printerDPI} DPI");
        }

        private DrawingVisual RenderLabelPart(bool renderDynamic)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                double scale = _printerDPI / 96.0;
                dc.PushTransform(new TranslateTransform(scale, scale));
                
                foreach (var element in _label.LabelElements)
                {
                    bool isDynamic = element is IDynamicElement;
                    if (renderDynamic == isDynamic)
                    {
                        element.Draw(dc, _printerDPI);
                    }
                }
                dc.Pop();
            }
            return visual;
        }

        private DrawingVisual DrawDynamicLabelPart(string fieldTag)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                foreach (var element in _label.LabelElements.OfType<IDynamicElement>())
                {
                    if (element.Name == fieldTag) 
                        ((LabelElement)element).Draw(dc, _printerDPI);
                }
            }
            return visual;
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
