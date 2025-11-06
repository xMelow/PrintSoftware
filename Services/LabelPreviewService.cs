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

        //public BitmapSource RenderLabelPreview()
        //{
        //    

        //    if (_labelBitmap == null)
        //    {
        //        _labelBitmap = new RenderTargetBitmap(
        //                    (int)pixelWidth,
        //                    (int)pixelHeight,
        //                    dpi,
        //                    dpi,
        //                    PixelFormats.Pbgra32);
        //    }

        //    if (_staticVisual == null) 
        //        _staticVisual = RenderLabelPart(scale, renderDynamic: false);

        //   

        //    _labelBitmap.Render(_staticVisual);
        //    _labelBitmap.Render(_dynamicVisual);
        //    return _labelBitmap;
        //}

        //public BitmapSource RenderLabelPreview()
        //{
        //    // 

        //    return
        //}

        //public BitmapSource RenderLabelPreview()
        //{
        //    int scale = 12;
        //    int dpi = 96;

        //    double maxX = _currentLabel.LabelElements.Max(e => e.Xend > 0 ? e.Xend : e.X);
        //    double maxY = _currentLabel.LabelElements.Max(e => e.Yend > 0 ? e.Yend : e.Y);
        //    int pixelWidth = (int)(maxX * scale);
        //    int pixelHeight = (int)(maxY * scale);

        //    // Create bitmap only once unless size changes
        //    if (_labelBitmap == null || _labelBitmap.PixelWidth != pixelWidth || _labelBitmap.PixelHeight != pixelHeight)
        //    {
        //        _labelBitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, dpi, dpi, PixelFormats.Pbgra32);
        //    }

        //    // Only rebuild static layer if required
        //    if (_staticVisual == null)
        //        _staticVisual = RenderLabelPart(scale, renderDynamic: false);

        //    // Rebuild dynamic layer (but reuse existing DrawingVisual object)
        //    if (_dynamicVisual == null)
        //        _dynamicVisual = new DrawingVisual();

        //    using (var dc = _dynamicVisual.RenderOpen())
        //    {
        //        foreach (var element in _currentLabel.LabelElements)
        //        {
        //            if (!string.IsNullOrEmpty(element.VariableName))
        //                element.Draw(dc, scale);
        //        }
        //    }

        //    // Clear and re-render both layers
        //    _labelBitmap.Clear(); // ⚠️ WPF doesn’t have a direct Clear(), see note below
        //    _labelBitmap.Render(_staticVisual);
        //    _labelBitmap.Render(_dynamicVisual);

        //    return _labelBitmap;
        //}
    }
}
