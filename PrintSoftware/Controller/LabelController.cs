using PrintSoftware.Domain.Label;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using PrintSoftware.Domain;
using PrintSoftware.Services;

namespace PrintSoftware.Controller
{
    public class LabelController
    {
        private readonly LabelService _labelService;

        public LabelController()
        {
            _labelService = new LabelService();
        }

        public Label GetLabel()
        {
            return _labelService.CurrentLabel;
        }

        public void CreateLabel(string labelName)
        {
            _labelService.CreateLabel(labelName);
        }

        public void UpdateLabelData(string name, string data)
        {
            _labelService.UpdateLabelDataElement(name, data);
        }

        public Label UpdateLabelData(Dictionary<string, string> labelData)
        {
            return _labelService.UpdateLabelData(labelData);
        }

        public Label UpdateLabelDataFromRow(DataRow row)
        {
           return _labelService.UpdateLabelDataFromRow(row);
        }
    }
}
