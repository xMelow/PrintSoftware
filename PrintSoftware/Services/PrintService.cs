using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PrintSoftware.Domain.Label;

namespace PrintSoftware.Services
{
    public class PrintService
    {
        private readonly string settingsTsplTemplate = @"
                SIZE 110 mm, 110 mm
                GAP {Gap} mm, 0 mm
                REFERENCE 0,0
                SPEED {Speed}
                DENSITY {Density}
                SET RIBBON ON
                SET PEEL OFF
                SET CUTTER {Cutter}
                SET PARTIAL_CUTTER OFF
                SET TEAR {Tear}
                SET REWIND OFF
                DIRECTION {Direction}
                SHIFT 0,0
                OFFSET {Offset} mm
            ";

        public Dictionary<string, string> CurrentSettings { get; private set; }

        public PrintService()
        {
            CurrentSettings = new Dictionary<string, string>
            {
                { "Speed", "1" },
                { "Density", "11"},
                { "Direction", "0" },
                { "Gap", "3" },
                { "Cutter", "ON" },
                { "Offset", "0" },
                { "Tear", "ON" },
            };
        }

        public void SetPrintSettings(Dictionary<string, string> printSettings)
        {
            foreach (var setting in printSettings)
            {
                CurrentSettings[setting.Key] = setting.Value;
            }
        }

        public void PrintLabel(Label label, int amount = 1)
        {
            string settingsTspl = settingsTsplTemplate;
            foreach (var setting in CurrentSettings)
            {
                settingsTspl = settingsTspl.Replace("{" + setting.Key + "}", setting.Value);
            }

            string labelTspl = label.CreateLabelTspl();
            labelTspl += $"PRINT {amount},1" + "\r\n";
            
            SendTsplIp(settingsTspl + labelTspl);
        }

        private void SendTsplIp(string tspl)
        {
            // string printerFlorIp = "192.168.1.133";
            // string printerKen = "192.168.0.53";
            string printerFlorHostname = "PRN-Gunny";
            int port = 9100;

            Console.WriteLine(tspl);

            try
            {
                using (TcpClient client = new TcpClient(printerFlorHostname, port))
                {
                    byte[] data = Encoding.ASCII.GetBytes(tspl);
                    var stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending label to printer: {ex.Message}");
            }
        }
    }
}
