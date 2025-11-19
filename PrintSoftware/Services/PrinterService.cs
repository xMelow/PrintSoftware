using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.Http;
using PrintSoftware.Domain.Printer;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrintSoftware.Services;

public class PrinterService
{
    public PrinterService() { }
    
    public async Task<List<PrinterInfo>> ScanLocalSubnetForPrintersAsync(int timeoutMs = 300, int maxConcurrency = 200)
    {
        var local = GetLocalIPv4Address();
        if (local == null) return new List<PrinterInfo>();

        var bytes = local.GetAddressBytes();
        var baseIp = new IPAddress(new byte[] { bytes[0], bytes[1], 1, 1 });
        var endIp = new IPAddress(new byte[] { bytes[0], bytes[1], 1, 254 });

        return await ScanIpRangeForPrintersAsync(baseIp.ToString(), endIp.ToString(), timeoutMs, maxConcurrency);
    }
    
    private IPAddress? GetLocalIPv4Address()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a));
        return ip;
    }

    private async Task<List<PrinterInfo>> ScanIpRangeForPrintersAsync(string startIp, string endIp, int timeoutMs = 300, int maxConcurrency = 200)
    {
        var result = new ConcurrentBag<PrinterInfo>();

        if (!IPAddress.TryParse(startIp, out var start)) throw new ArgumentException("Invalid start IP", nameof(startIp));
        if (!IPAddress.TryParse(endIp, out var end)) throw new ArgumentException("Invalid end IP", nameof(endIp));

        const int port = 9100;

        var addresses = GetAddressRange(start, end).ToList();

        using var sem = new SemaphoreSlim(maxConcurrency);
        var tasks = addresses.Select(async ip =>
        {
            await sem.WaitAsync();
            try
            {
                var open = await ProbePrinterPortsAsync(ip, port, timeoutMs);
                if (open.Any())
                {
                    
                    result.Add(new PrinterInfo
                    {
                        Model = "Unknown",
                        IP = ip.ToString(),
                        MAC = "Unknown",
                        Source = new IPEndPoint(ip, port),
                    });
                }
            }
            finally
            {
                sem.Release();
            }
        });

        await Task.WhenAll(tasks);
        return result.OrderBy(p => p.IP).ToList();
    }

    private IEnumerable<IPAddress> GetAddressRange(IPAddress start, IPAddress end)
    {
        if (start.AddressFamily != AddressFamily.InterNetwork || end.AddressFamily != AddressFamily.InterNetwork)
            throw new ArgumentException("Only IPv4 is supported for range scanning.");

        uint s = IpToUint(start);
        uint e = IpToUint(end);
        if (e < s) throw new ArgumentException("End IP must be greater than or equal to start IP");

        for (uint cur = s; cur <= e; cur++)
        {
            yield return UintToIp(cur);
        }
    }

    private async Task<List<int>> ProbePrinterPortsAsync(IPAddress ip, int port, int timeoutMs)
    {
        var open = new List<int>();
        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        try
        {
            var connectTask = client.ConnectAsync(new IPEndPoint(ip, port));
            var completed = await Task.WhenAny(connectTask, Task.Delay(timeoutMs));
            if (completed == connectTask && client.Connected)
            {
                open.Add(port);
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                    // throw exception
                }
            }
        }
        catch
        {
            // throw exception
        }
        return open;
    }
    
    private static uint IpToUint(IPAddress ip)
    {
        var bytes = ip.GetAddressBytes();
        if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
        return BitConverter.ToUInt32(bytes, 0);
    }

    private static IPAddress UintToIp(uint val)
    {
        var bytes = BitConverter.GetBytes(val);
        if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
        return new IPAddress(bytes);
    }
}
