using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace SimpleProject.Domain.Labels
{
    public class RectangleElement : LabelElement
    {
        public Brush Fill { get; set; } = Brushes.Transparent;
        public Pen Border { get; set; } = new Pen(Brushes.Black, 1);
        public int Xend { get; set; }
        public int Yend { get; set; }
        public int LineThinkness { get; set; } = 6;
        
        public override void Draw(DrawingContext dc, double scale)
        {
            dc.DrawRectangle(Fill, Border, new Rect(X * scale, Y * scale, Width * scale, Height * scale));
        }

        //BOX 33,178,1232,890,6
        public override string CreateTspl()
        {
            return $"BOX {X},{Y},{Xend},{Yend},{LineThinkness}";
        }
    }
}
