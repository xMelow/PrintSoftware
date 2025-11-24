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

        public void SetLabel(Label label)
        {
            _labelService.CurrentLabel = label;
        }

        public List<Label> GetAllLabels()
        {
            return _labelService.GetAllLabels();
        }

        public Label GetCurrentLabel()
        {
            return _labelService.CurrentLabel;
        }

        public List<LabelField> GetLabelFields()
        {
            return _labelService.GetLabelFields();
        }

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
