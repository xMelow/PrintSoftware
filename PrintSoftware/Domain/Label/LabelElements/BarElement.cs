using PrintSoftware.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class BarElement : LabelElement
    {
        public Brush Stroke { get; set; } = Brushes.Black;
        public int XEend { get; set; }
        public int YEnd { get; set; }

        public BarElement()
        {
            Type = "BAR";
        }

        public BarElement(int x, int y, int xEnd, int yEnd)
        {
            Type = "BAR";
            X = x;
            Y = y;
            XEend = xEnd;
            YEnd = yEnd;
        }
        
        public override void Draw(DrawingContext dc, double scale)
        {
            dc.DrawRectangle(Stroke, null, new Rect(X * scale, Y * scale, XEend * scale, YEnd * scale));
        }
        
        public override string GetTspl()
        {
            // BAR 33,135,1199,7
            return $"{Type} {X},{Y},{XEend},{YEnd}";
        }
    }
}
