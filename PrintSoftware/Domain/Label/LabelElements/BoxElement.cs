using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class BoxElement : LabelElement
    {
        public int XEnd { get; set; }
        public int YEnd { get; set; }
        public int Thickness { get; set; }
        public Brush Fill { get; } = Brushes.Transparent;
        public Pen Border { get; } = new Pen(Brushes.Black, 1);

        public BoxElement()
        {
            Type = "BOX";
        }
        
        public BoxElement(int x, int y, int xEnd, int yEnd, int thickness)
        {
            Type = "BOX";
            X = x;
            Y = y;
            XEnd = xEnd;
            YEnd = yEnd;
            Thickness = thickness;
        }

        public override void Draw(DrawingContext dc, int dpi)
        {
            // double scale = dpi / 96.0;
            
            var width = XEnd - X;
            var height = YEnd - Y;
            
            Rect rect = new Rect(X, Y, width, height);
            Pen pen = new Pen(Border.Brush, Border.Thickness);
            
            dc.DrawRectangle(Fill, pen, rect);
        }
        
        //BOX 33,178,1232,890,6 -> data and tspl
        public override string GetTspl()
        {
            return $"{Type} {X},{Y},{XEnd},{YEnd},{Thickness}";
        }
    }
}
