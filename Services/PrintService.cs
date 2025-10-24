using SimpleProject.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleProject.Services
{
    public class PrintService
    {
        private readonly string settingsTsplTemplate = @"
                SIZE 105.6 mm, 110 mm
                GAP {Gap} mm,0 mm
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
            labelTspl += $"PRINT {amount},1";
            SendTspl(settingsTspl + labelTspl);
        }

        private void SendTspl(string tspl)
        {
            string ip = "192.168.1.113";
            int port = 9100;
            Debug.WriteLine(tspl);
            try
            {
                using (TcpClient client = new TcpClient(ip, port))
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
