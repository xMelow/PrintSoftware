using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows;

namespace SimpleProject.Domain.Labels
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
        
        public CircleElement(int radius, int x, int y, int thickness)
        {
            Type = "CIRCLE";
            X = x;
            Y = y;
            Radius = radius;
            Thickness = thickness;
        }

        public override void Draw(DrawingContext dc, double scale)
        {
            dc.DrawEllipse(Fill, new Pen(Stroke, Thickness * scale), new Point(X * scale, Y * scale), Radius * 3, Radius * 3);
        }
        public override string GetTspl()
        {
            //CIRCLE 933,590,260,12
            return $"{Type} {X},{Y},{Radius},{Thickness}";
        }
    }
}
