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
        private readonly int _scale = 1;
        private readonly int _printerDPI = 300;
        private int labelWidthInDots;
        private int labelHeightInDots;
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
            return new RenderTargetBitmap(
                labelWidthInDots,
                labelHeightInDots,
                _printerDPI,
                _printerDPI,
                PixelFormats.Pbgra32
            );
        }
        private void CalculateLabelPreviewPixels()
        {
            // calculate label width and height from mm to dots
            // set label preview width and height
            labelWidthInDots = (int)((_label.Width * _printerDPI) / 25.4);
            labelHeightInDots = (int)((_label.Height * _printerDPI) / 25.4);
            
            Console.WriteLine($"Preview size: {labelWidthInDots}x{labelHeightInDots}px at {_printerDPI} DPI and scale = {_scale}");
        }

        private DrawingVisual RenderLabelPart(bool renderDynamic)
        {
            var visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                foreach (var element in _label.LabelElements)
                {
                    bool isDynamic = element is IDynamicElement;
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
                foreach (var element in _label.LabelElements.OfType<IDynamicElement>())
                {
                    if (element.Name == fieldTag) 
                        ((LabelElement)element).Draw(dc, _scale);
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
