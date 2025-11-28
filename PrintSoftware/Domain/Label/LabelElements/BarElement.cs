using PrintSoftware.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class BarElement : LabelElement
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public BarElement()
        {
            Type = "BAR";
        }

        public BarElement(int x, int y, int width, int height)
        {
            Type = "BAR";
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        
        public override void Draw(DrawingContext dc, int dpi)
        {
           //TODO: LINE THICKNESS
           // double scale = dpi / 96.0;
           
            var line = new Pen(Brushes.Black, 1);
            Pen pen = new Pen(line.Brush, line.Thickness);
            
            Point startPoint = new Point(X , Y);
            Point endPoint = new Point((X + Width), Y);

            dc.DrawLine(pen, startPoint, endPoint);
        }
        
        public override string GetTspl()
        {
            // BAR 33,135,1199,7
            return $"{Type} {X},{Y},{Width},{Height}";
        }
    }
}
