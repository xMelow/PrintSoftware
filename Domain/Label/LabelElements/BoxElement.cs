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
        public int Radius { get; set; }
        public Brush Fill { get; } = Brushes.Transparent;
        public Pen Border { get; } = new Pen(Brushes.Black, 1);

        public BoxElement()
        {
            Type = "BOX";
        }
        
        public BoxElement(int x, int y, int xEnd, int yEnd, int radius)
        {
            Type = "BOX";
            X = x;
            Y = y;
            XEnd = xEnd;
            YEnd = yEnd;
            Radius = radius;
        }

        public override void Draw(DrawingContext dc, double scale)
        {
            double rectWidth = XEnd - X;
            double rectHeight = YEnd - Y;

            // if (rectWidth == 0) rectWidth = 1;
            // if (rectHeight == 0) rectHeight = 1;

            double xPx = X * scale;
            double yPx = Y * scale;
            double widthPx = rectWidth * scale;
            double heightPx = rectHeight * scale;
            
            // Console.WriteLine($"Rect: xP={xPx}, yP={yPx}, X={X}, Y={Y}, XEnd={XEnd}, YEnd={YEnd}, widthPx={widthPx}, heightPx={heightPx} rectWidth={rectWidth}, rectHeight={rectHeight}");

            dc.DrawRectangle(Fill, new Pen(Border.Brush, Border.Thickness * scale * 5), new Rect(xPx, yPx, widthPx, heightPx));

        }

        //BOX 33,178,1232,890,6 -> data and tspl
        public override string GetTspl()
        {
            return $"{Type} {X},{Y},{XEnd},{YEnd},{Radius}";
        }
    }
}
