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
    public class RectangleElement : LabelElement
    {
        public Brush Fill { get; set; } = Brushes.Transparent;
        public int LineThinkness { get; set; } = 6;
        public Pen Border { get; set; } = new Pen(Brushes.Black, 1);

        public override void Draw(DrawingContext dc, double scale)
        {
            double rectWidth = Xend - X;
            double rectHeight = Yend - Y;

            if (rectWidth == 0) Width = 1;
            if (rectHeight == 0) Height = 1;

            double xPx = X * scale;
            double yPx = Y * scale;
            double widthPx = rectWidth * scale;
            double heightPx = rectHeight * scale;

            dc.DrawRectangle(Fill, new Pen(Border.Brush, Border.Thickness * scale * 5), new Rect(xPx, yPx, widthPx, heightPx));

            //Debug.WriteLine($"Rect: xP={xPx}, yP={yPx}, widthPx={widthPx}, heightPx={heightPx}");
            //Debug.WriteLine($"Rect: X={X}, Y={Y}, W={Width}, H={Height}, Xend={Xend}, Yend={Yend}");
        }

        //BOX 33,178,1232,890,6 -> data and tspl
        public override string CreateTspl()
        {
            return $"BOX {X},{Y},{Xend},{Yend},{LineThinkness}";
        }

        public override void UpdateContent(string newValue)
        {
            throw new NotImplementedException();
        }
    }
}
