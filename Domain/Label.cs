using SimpleProject.Domain.Labels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleProject.Domain
{
    public class Label
    {

        // CLS 
        // TEXT 427,47,""0"",0,16,16,""{Title}"" 
        // BAR 33,135,1199,7 
        // BOX 33,178,1232,890,6 
        // QRCODE 938,951,L,14,A,0,M2,S7,""{QR}"" 
        // CIRCLE 933,590,260,12 
        // TEXT 77,234,""0"",0,16,16,""Name:"" 
        // TEXT 77,322,""0"",0,16,16,""Phone number:"" 
        // TEXT 77,413,""0"",0,16,16,""Email:"" 
        // TEXT 77,505,""0"",0,16,16,""Company:"" 
        // TEXT 295,231,""0"",0,16,16,""{Name}"" 
        // TEXT 509,320,""0"",0,16,16,""{PhoneNumber}"" 
        // TEXT 295,410,""0"",0,16,16,""{Email}"" 
        // TEXT 396,502,""0"",0,16,16,""{Company}"" 
        // PRINT 1,1 
        // ";
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Unit { get; set; }

        public List<LabelElement> LabelElements { get; set; } = new List<LabelElement>();

        public void Draw(DrawingContext dc, double scale)
        {
            foreach(var element in LabelElements)
            {
                element.Draw(dc, scale);
            }
        }

        public void UpdateLabelData(Dictionary<string, string> labelData)
        {
            foreach (LabelElement element in LabelElements)
            {
                if (!string.IsNullOrEmpty(element.VariableName) &&
                    labelData.TryGetValue(element.VariableName, out string? newValue))
                {
                    element.UpdateContent(newValue);
                }
            }
        }

        public string CreateLabelTspl()
        {
            var sb = new StringBuilder();
            sb.AppendLine("CLS");

            foreach (LabelElement element in LabelElements)
            {
                sb.AppendLine(element.CreateTspl());
            }
            return sb.ToString();
        }
    }
}
