using PrintSoftware.Domain.Printer;
using PrintSoftware.Services;

namespace PrintSoftware.Controller;

public class PrinterController
{
    private readonly PrinterService _printerService;
    
    public PrinterController()
    {
        _printerService = new PrinterService();
    }
    
    public async Task<List<PrinterInfo>> DiscoverPrintersOnNetwork()
    {
        var devices = await _printerService.ScanLocalSubnetForPrintersAsync(300);
        foreach (var d in devices)
        {
            Console.WriteLine($"{d.IP} - {d.Model}");
        }
        return await _printerService.ScanLocalSubnetForPrintersAsync();
    }
}