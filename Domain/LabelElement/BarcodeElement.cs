using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleProject.Domain.Labels
{
    public class BarcodeElement : LabelElement
    {
        public string Type { get; set; }
        public string Content { get; set; }

        public override void Draw(DrawingContext dc, double scale)
        {
            throw new NotImplementedException();
        }
        public override string CreateTspl()
        {
            return $"BARCODE {X},{Y},\"128\",{Height},1,0,2,2,\"{Content}\"";
        }

        public override void UpdateContent(string newValue)
        {
            Content = newValue;
        }
    }
}
