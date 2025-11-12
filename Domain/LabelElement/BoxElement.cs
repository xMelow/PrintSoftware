using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SimpleProject.Domain.Labels
{
    public class BoxElement : LabelElement
    {
        public int XEnd { get; }
        public int YEnd { get; }
        public int Radius { get; }
        public Brush Fill { get; } = Brushes.Transparent;
        public Pen Border { get; } = new Pen(Brushes.Black, 1);

        public BoxElement(int xEnd, int yEnd, int radius)
        {
            XEnd = xEnd;
            YEnd = yEnd;
            Radius = radius;
        }

        public override void Draw(DrawingContext dc, double scale)
        {
            double rectWidth = XEnd - X;
            double rectHeight = YEnd - Y;

            // if (rectWidth == 0) Width = 1;
            // if (rectHeight == 0) Height = 1;

            double xPx = X * scale;
            double yPx = Y * scale;
            double widthPx = rectWidth * scale;
            double heightPx = rectHeight * scale;

            dc.DrawRectangle(Fill, new Pen(Border.Brush, Border.Thickness * scale * 5), new Rect(xPx, yPx, widthPx, heightPx));

            //Debug.WriteLine($"Rect: xP={xPx}, yP={yPx}, widthPx={widthPx}, heightPx={heightPx}");
            //Debug.WriteLine($"Rect: X={X}, Y={Y}, W={Width}, H={Height}, Xend={Xend}, Yend={Yend}");
        }

        //BOX 33,178,1232,890,6 -> data and tspl
        public override string GetTspl()
        {
            return $"{Type} {X},{Y},{XEnd},{YEnd},{Radius}";
        }
    }
}
