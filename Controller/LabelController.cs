using SimpleProject.Domain;
using SimpleProject.Domain.Labels;
using SimpleProject.Services;
using System;
using System.Collections.Generic;
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

        public Label CreateLabelWithData(Dictionary<string, string> labelData)
        {
            Label label = _labelService.CreateLabel(labelData);
            return label;
        }

        public Label CreateLabelWithElements(string title, string name, string phone, string email, string company)
        {
            return _labelService.CreateLabelWithElements(title, name, phone, email, company);
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
