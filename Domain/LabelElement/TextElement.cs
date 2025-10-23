using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SimpleProject.Domain.Labels
{
    public class TextElement : LabelElement
    {
        public string? Text { get; set; }
        public string FontFamily { get; set; } = "Arial";
        public double FontSize { get; set; } = 12;
        public Brush Color { get; set; } = Brushes.Black;

        public override void Draw(DrawingContext dc, double scale, double dpi)
        {
            var textToDraw = Text ?? string.Empty;

            var formattedText = new FormattedText(
                textToDraw,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily),
                FontSize * scale,
                Color,
                dpi / 96.0);

            double xPx = (X / 25.4) * dpi * scale;
            double yPx = (Y / 25.4) * dpi * scale;

            dc.PushTransform(new TranslateTransform(xPx, yPx));
            dc.DrawText(formattedText, new Point(0, 0));
            dc.Pop();
        }

        //TEXT 77,505,""0"",0,16,16,""Company:""
        //TEXT x,y, « font « ,rotation,x-multiplication,y-multiplication,[alignment,] « content «
        public override string CreateTspl()
        {
            var content = Text ?? string.Empty;
            return $"TEXT {X},{Y},\"0\",0,{FontSize},{FontSize},\"{content}\"";
        }

        public override void UpdateContent(string newValue)
        {
            Text = newValue;
        }
    }
}
