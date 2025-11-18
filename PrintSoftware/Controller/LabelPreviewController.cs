using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PrintSoftware.Domain;
using PrintSoftware.Domain.Label;
using PrintSoftware.Services;

namespace PrintSoftware.Controller
{
    public class LabelPreviewController
    {
        private readonly LabelPreviewService _labelPreviewService;

        public LabelPreviewController()
        {
            _labelPreviewService = new LabelPreviewService();
        }

        public void SetLabel(Label label)
        {
            _labelPreviewService.SetLabel(label);
        }

        public RenderTargetBitmap CreateNewPreview()
        {
            return _labelPreviewService.CreateNewPreview();
        }
        
        public RenderTargetBitmap CreateLabelPreview()
        {
            return _labelPreviewService.CreateLabelPreview();
        }
        
        public BitmapSource RenderDynamicElements()
        {
            return _labelPreviewService.RenderDynamicLabelElements();
        }
        
        public BitmapSource RenderDynamicElement(string fieldTag)
        {
            return _labelPreviewService.RenderDynamicLabelElement(fieldTag);
        }
        
        public BitmapSource RenderStaticElements()
        {
            return _labelPreviewService.RenderStaticLabelPreview();
        }
    }
}
