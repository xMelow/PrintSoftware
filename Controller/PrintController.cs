using SimpleProject.Domain;
using SimpleProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProject.Controller
{
    public class PrintController
    {
        private readonly PrintService _printService;
        private readonly LabelService _labelService;

        public PrintController()
        {
            _printService = new PrintService();
        }

        public Dictionary<string, string> GetPrinterSettings()
        {
            return new Dictionary<string, string>(_printService.CurrentSettings);
        }

        public void UpdatePrinterSettings(Dictionary<string, string> newSettings)
        {
            _printService.SetPrintSettings(newSettings);
        }

        public void Print(Dictionary<string, string> labelData)
        {
            Label label = _labelService.updateLabel();
            //Label label = new Label(labelData);
            _printService.PrintLabel(label);
        }

        public void SetPrintSettings(Dictionary<string, string> printSettings)
        {
            _printService.SetPrintSettings(printSettings);
        }
    }
}
