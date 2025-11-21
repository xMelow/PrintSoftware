using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintSoftware.Domain;
using PrintSoftware.Domain.Label;
using PrintSoftware.Interfaces;
using PrintSoftware.Services;

namespace PrintSoftware.Controller
{
    public class PrintController : IPrintController
    {
        private readonly PrintService _printService;
        public PrintController()
        {
            _printService = new PrintService();
        }

        public Dictionary<string, string> GetPrinterSettings()
        {
            return new Dictionary<string, string>(_printService.CurrentSettings);
        }

        public void SetPrinterSettings(Dictionary<string, string> printSettings)
        {
            _printService.SetPrintSettings(printSettings);
        }

        public void PrintLabel(Label label, int amount)
        {
            _printService.PrintLabel(label, amount);
        }

    }
}
