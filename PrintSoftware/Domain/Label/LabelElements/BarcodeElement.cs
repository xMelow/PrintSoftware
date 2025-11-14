using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using PrintSoftware.Interfaces;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class BarcodeElement : LabelElement, IDynamicElement
    {
        public string Content { get; set; }
        public string? Name { get; set; }

        public override void Draw(DrawingContext dc, double scale)
        {
            throw new NotImplementedException();
        }
        public override string GetTspl()
        {
            throw new NotImplementedException();
        }

        public void UpdateContent(string newValue)
        {
            Content = newValue;
        }
    }
}
