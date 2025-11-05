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
        private DrawingVisual _staticVisual;
        private DrawingVisual _dynamicVisual;
        private RenderTargetBitmap _cachedBitmap;

        public LabelPreviewService() { }

        private DrawingVisual RenderLabelPart(Label label, double scale, bool renderDynamic)
        {
            DrawingVisual visual = new DrawingVisual();
            using (var dc = visual.RenderOpen())
            {
                foreach (var element in label.LabelElements)
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

        //public BitmapSource RenderDynamicLabelPreview()
        //{
        //    return
        //}

        //public BitmapSource RenderStaticLabelPreview()
        //{
        //    return 
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


        //public BitmapSource RenderLabelPreview()
        //{
        //    int scale = 12;
        //    int dpi = 96;

        //    // move to label for faster loading
        //    double maxX = _currentLabel.LabelElements.Max(e => e.Xend > 0 ? e.Xend : e.X);
        //    double maxY = _currentLabel.LabelElements.Max(e => e.Yend > 0 ? e.Yend : e.Y);

        //    double pixelWidth = maxX * scale;
        //    double pixelHeight = maxY * scale;

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

        //    _dynamicVisual = RenderLabelPart(scale, renderDynamic: true);

        //    _labelBitmap.Render(_staticVisual);
        //    _labelBitmap.Render(_dynamicVisual);
        //    return _labelBitmap;
        //}
    }
}
