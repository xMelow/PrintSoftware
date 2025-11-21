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
using PrintSoftware.Interfaces;
using PrintSoftware.Services;

namespace PrintSoftware.Controller
{
    public class LabelController : ILabelController
    {
        private readonly LabelService _labelService;

        public LabelController()
        {
            _labelService = new LabelService();
        }

        public Label? GetLabel(string labelName)
        {
            return _labelService.GetLabel(labelName);
        }

        public Label GetCurrentLabel()
        {
            return _labelService.CurrentLabel;
        }

        // public Label GetJsonLabel(string labelName)
        // {
        //     return _labelService.GetJsonLabel(labelName);
        // }

        public void UpdateLabelElementData(string name, string data)
        {
            _labelService.UpdateLabelDataElement(name, data);
        }

        public void UpdateLabelDataFromRow(DataRow row)
        {
           _labelService.UpdateLabelDataFromRow(row);
        }
    }
}
