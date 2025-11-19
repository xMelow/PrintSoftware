using System.Net;

namespace PrintSoftware.Domain.Printer;

public class PrinterInfo
{
    public string PrinterName { get; set; }
    public string Model { get; set; }
    public string IP { get; set; }
    public string MAC { get; set; }
    public IPEndPoint Source { get; set; }
    
    public PrinterInfo() {}
}