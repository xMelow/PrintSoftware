using PrintSoftware.Domain.Label;

namespace PrintSoftware.Interfaces;

public interface IPrintController
{
    Dictionary<string, string> GetPrinterSettings();
    void SetPrinterSettings(Dictionary<string, string> printerSettings);
    void PrintLabel(Label label, int amount);
}