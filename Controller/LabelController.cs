using SimpleProject.Domain;
using SimpleProject.Domain.Labels;
using SimpleProject.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SimpleProject.Controller
{
    public class LabelController
    {
        private readonly LabelService _labelService;
        private readonly PrintService _printService;

        public LabelController()
        {
            _labelService = new LabelService();
            _printService = new PrintService();
        }

        public Label GetLabel()
        {
            return _labelService.GetLabel();
        }

        public Label UpdateLabelWithData(Dictionary<string, string> labelData)
        {
            return _labelService.UpdateLabelData(labelData);
        }

        public BitmapSource GetPreview(int scale = 3)
        {
            return _labelService.RenderLabelPreview(scale);
        }

        public void Printlabel(Label label)
        {
            _printService.PrintLabel(label);
        }
    }
}
