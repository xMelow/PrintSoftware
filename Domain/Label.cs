using SimpleProject.Domain.Labels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        //private readonly Dictionary<string, (double X, double Y, string Type)> elementTemplate = new Dictionary<string, (double, double, string)>
        //    {
        //        {"Title", (427, 47, "Text")},
        //        {"NameLabel", (77, 234, "Text")},
        //        {"PhoneLabel", (77, 322, "Text")},
        //        {"EmailLabel", (77, 413, "Text")},
        //        {"CompanyLabel", (77, 505, "Text")},
        //        {"Name", (295, 231, "Text")},
        //        {"PhoneNumber", (509, 320, "Text")},
        //        {"Email", (295, 410, "Text")},
        //        {"Company", (396, 502, "Text")},
        //        {"QR", (938, 951, "QRCode")},
        //        {"Circle", (933, 590, "Circle")},
        //        {"Bar", (33, 135, "Bar")},
        //        {"Box", (33, 178, "Box")}
        //    };
        public string Name { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Unit { get; set; }

        public List<LabelElement> LabelElements { get; set; } = new List<LabelElement>();

        //public Label(Dictionary<string, string> labeldata)
        //{
        //    CreateElements(labeldata);
        //}

        public Label() { }

        public void Draw(DrawingContext dc, double scale)
        {
            foreach(var element in LabelElements)
            {
                element.Draw(dc, scale);
            }
        }

        //public void CreateElements(Dictionary<string, string> labelData)
        //{
        //    LabelElements.Clear();

        //    foreach (var kvp in elementTemplate)
        //    {
        //        var key = kvp.Key;
        //        var (x, y, type) = kvp.Value;

        //        LabelElement element = type switch
        //        {
        //            "QRCode" => new QRCodeElement { X = x, Y = y, Content = labelData.GetValueOrDefault("QR", "") },
        //            "Circle" => new CircleElement { X = x, Y = y, Radius = 260, Thickness = 12 },
        //            "Bar" => new LineElement { X = x, Y = y, Width = 1199, Height = 7, Thickness = 6 },
        //            "Box" => new RectangleElement { X = x, Y = y, Xend = 1232, Yend = 890, Width = 1199, Height = 712, LineThinkness = 6 },
        //            _ => new TextElement { X = x, Y = y, Text = labelData.GetValueOrDefault(key, GetStaticTextLabel(key)), FontSize = 16 }
        //        };
        //        Elements.Add(element);
        //    }
        //}
        //private string GetStaticTextLabel(string key)
        //{
        //    return key switch
        //    {
        //        "NameLabel" => "Name:",
        //        "PhoneLabel" => "Phone number:",
        //        "EmailLabel" => "Email:",
        //        "CompanyLabel" => "Company",
        //        _ => string.Empty
        //    };
        //} 

        public string CreateLabelTspl()
        {
            var sb = new StringBuilder();
            sb.AppendLine("CLS");

            foreach (LabelElement element in LabelElements)
            {
                sb.AppendLine(element.CreateTspl());
            }
            sb.AppendLine("PRINT 1,1");
            return sb.ToString();
        }
    }
}
