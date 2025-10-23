using SimpleProject.Domain.Labels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;

namespace SimpleProject.Domain.Labels
{
    public class LineElement : LabelElement
    {
        public Brush Stroke { get; set; } = Brushes.Black;

        public override string CreateTspl()
        {
            // BAR 33,135,1199,7 -> data and tspl
            return $"BAR {X},{Y},{Width},{Height}";
        }

        public override void Draw(DrawingContext dc, double scale, double dpi)
        {
            dc.DrawRectangle(Stroke, null, new Rect(X * scale, Y * scale, Width * scale, Height * scale));
        }

        public override void UpdateContent(string newValue)
        {
            throw new NotImplementedException();
        }
    }
}
