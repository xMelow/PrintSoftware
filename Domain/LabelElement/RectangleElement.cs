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
        public Brush Fill { get; set; } = Brushes.LightBlue;
        public int Xend { get; set; }
        public int Yend { get; set; }
        public int LineThinkness { get; set; } = 6;
        public Pen Border { get; set; } = new Pen(Brushes.Black, 1);


        public override void Draw(DrawingContext dc, double scale, double dpi)
        {
            Debug.WriteLine($"Rect: X={X}, Y={Y}, W={Width}, H={Height}");

            double xPx = (X / 25.4) * dpi * scale;
            double yPx = (Y / 25.4) * dpi * scale;
            double widthPx = (Width / 25.4) * dpi * scale;
            double heightPx = (Height / 25.4) * dpi * scale;
            var border = new Pen(Border.Brush, Border.Thickness * dpi / 96.0);

            dc.DrawRectangle(Fill, border, new Rect(xPx, yPx, widthPx, heightPx));
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
