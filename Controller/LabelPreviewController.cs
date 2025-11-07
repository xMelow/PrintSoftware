using SimpleProject.Domain;
using SimpleProject.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimpleProject.Controller
{
    public class LabelPreviewController
    {
        private readonly LabelPreviewService _LabelPreviewService;

        public LabelPreviewController(Label label)
        {
            _LabelPreviewService = new LabelPreviewService(label);
        }

        public BitmapSource RenderDynamicElements()
        {
            return _LabelPreviewService.RenderDynamicLabelElements();
        }
        
        public BitmapSource RenderDynamicElement(string fieldTag)
        {
            return _LabelPreviewService.RenderDynamicLabelElement(fieldTag);
        }
        
        public BitmapSource RenderStaticElements()
        {
            return _LabelPreviewService.RenderStaticLabelPreview();
        }
    }
}
