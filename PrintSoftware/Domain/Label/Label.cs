using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;
using PrintSoftware.Domain.Label.LabelElements;
using PrintSoftware.Interfaces;

namespace PrintSoftware.Domain.Label
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
        public List<LabelElement> LabelElements { get; set; }

        public Label()
        {
            LabelElements = new List<LabelElement>();
        }
        
        public Label(string name)
        {
            Name = name;
            Unit = "mm";
            LabelElements = new List<LabelElement>();
        }

        public Label(string name, double width, double height, string unit)
        {
            Name = name;
            Width = width;
            Height = height;
            Unit = unit;
            LabelElements = new List<LabelElement>();
        }
        public Label(string name, double width, double height, string unit, List<LabelElement> labelElements)
        {
            Name = name;
            Width = width;
            Height = height;
            Unit = unit;
            LabelElements = labelElements;
        }

        public void Draw(DrawingContext dc, double scale)
        {
            foreach (var element in LabelElements)
            {
                element.Draw(dc, scale);
            }
        }

        public void UpdateLabelData(string fieldName, string data)
        {
            var element = LabelElements
                .OfType<IDynamicElement>()
                .FirstOrDefault(e => e.Name == fieldName);
            
            if  (element == null)
                throw new ArgumentException($"LabelElement: '{fieldName}' not found");
                
            element?.UpdateContent(data);
        }

        public void UpdateLabelData(Dictionary<string, string> labelData)
        {
            foreach (var element in LabelElements.OfType<IDynamicElement>())
            {
                if (!string.IsNullOrEmpty(element.Name) &&
                    labelData.TryGetValue(element.Name, out var newValue))
                    element.UpdateContent(newValue);
            }
        }

        public string CreateLabelTspl()
        {
            var tspl = "CLS" + "\n";

            foreach (LabelElement element in LabelElements)
            {
                tspl += element.GetTspl() + "\n";
            }
            
            return tspl;
        }
    }
}