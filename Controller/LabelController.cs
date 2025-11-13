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

        public LabelController()
        {
            _labelService = new LabelService();
        }

        public Label GetLabel()
        {
            return _labelService.CurrentLabel;
        }

        public Label UpdateLabelData(string name, string data)
        {
            return _labelService.UpdateLabelData(name, data);
        }

        public Label UpdateLabelData(Dictionary<string, string> labelData)
        {
            return _labelService.UpdateLabelData(labelData);
        }

        public Label CreateLabelFromRow(DataRow row)
        {
           return _labelService.UpdateLabelDataFromRow(row);
        }
    }
}
