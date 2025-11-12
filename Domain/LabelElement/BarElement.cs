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
    public class BarElement : LabelElement
    {
        private Brush Stroke { get; set; } = Brushes.Black;
        private int Width { get; set; }
        private int Height { get; set; }

        public BarElement(int x , int y, int width , int height)
        {
            Type = "BAR";
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public override void Draw(DrawingContext dc, double scale)
        {
            dc.DrawRectangle(Stroke, null, new Rect(X * scale, Y * scale, Width * scale, Height * scale));
        }
        
        public override string GetTspl()
        {
            // BAR 33,135,1199,7
            return $"{Type} {X},{Y},{Width},{Height}";
        }
    }
}
