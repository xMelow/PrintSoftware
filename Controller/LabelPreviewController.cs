using SimpleProject.Domain;
using SimpleProject.Services;
using System;
using System.Collections.Generic;
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

        public BitmapSource RenderDynamicLabelPreview()
        {
            return _LabelPreviewService.RenderDynamicLabelPreview();
        }

        public BitmapSource RenderStaticLabelPreview()
        {
            return _LabelPreviewService.RenderStaticLabelPreview();
        }

        public BitmapSource RenderFullLabelPreview()
        {
            return _LabelPreviewService.RenderFullLabelPreview();
        }

    }
}
