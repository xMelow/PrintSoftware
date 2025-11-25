using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class CircleElement : LabelElement
    {
        public int Radius { get; set; }
        public int Thickness { get; set; }
        public Brush Fill { get; set; } = Brushes.Transparent;
        public Brush Stroke { get; set; } = Brushes.Black;

        public CircleElement()
        {
            Type = "CIRCLE";
        }
        
        public CircleElement(int x, int y, int radius, int thickness)
        {
            Type = "CIRCLE";
            X = x;
            Y = y;
            Radius = radius;
            Thickness = thickness;
        }

        public override void Draw(DrawingContext dc, int dpi)
        {
            // double scaleFactor = dpi / 96.0;

            Point center = new Point(X, Y);
            double radius = Radius;
            double thickness = Thickness;

            dc.DrawEllipse(Fill, new Pen(Stroke, thickness), center, radius, radius);
        }

        public override string GetTspl()
        {
            //CIRCLE 933,590,260,12
            return $"{Type} {X},{Y},{Radius},{Thickness}";
        }
    }
}
