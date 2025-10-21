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
        public Brush Fill { get; set; } = Brushes.Transparent;
        public int Radius { get; set; } = 260;
        public int Thickness { get; set; } = 6;
        public Brush Stroke { get; set; } = Brushes.Black;

        public override void Draw(DrawingContext dc, double scale)
        {
            dc.DrawEllipse(Fill, new Pen(Stroke, Thickness * scale), new Point(X * scale, Y * scale), Radius * scale, Radius * scale);
        }
        public override string CreateTspl()
        {
            //CIRCLE 933,590,260,12
            return $"CIRCLE {X},{Y},{Radius},{Thickness}";
        }

        public override void UpdateContent(string newValue)
        {
            throw new NotImplementedException();
        }
    }
}
