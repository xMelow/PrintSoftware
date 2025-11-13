using SimpleProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SimpleProject.Domain.Labels;
using SimpleProject.Interfaces;

namespace SimpleProject.Services
{
    public class LabelPreviewService
    {
        private readonly Label _label;
        private readonly int _scale = 8;
        private readonly int _dpi = 50;
        private double _pixelWidth;
        private double _pixelHeight;
        private readonly RenderTargetBitmap _labelPreview;

        public LabelPreviewService(Label label) 
        {
            _label = label;
            CalculateLabelPreviewPixels();
            _labelPreview = CreateRenderTarget();
        }
        
        private void CalculateLabelPreviewPixels()
        {
            // label 
            // width = 110mm
            // height = 110mm
            
            // label preview window element
            // width = 400
            // height = 500
            
            double maxX = _label.LabelElements.Max(e => e.X);
            double maxY = _label.LabelElements.Max(e => e.Y);
            
            _pixelWidth = maxX * _scale;
            _pixelHeight = maxY * _scale;
            
            // _pixelWidth = Math.Round(_label.Width * (_dpi / 25.4));
            // _pixelHeight = Math.Round(_label.Height * (_dpi / 25.4));
            
            Console.WriteLine($"Preview size: {_pixelWidth}x{_pixelHeight}px at {_dpi} DPI and scale = {_scale}");
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
                    // TODO: non dynamic text elements not showing
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
