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

        public override void Draw(DrawingContext dc, double scale)
        {
            var textToDraw = Text ?? string.Empty;

            var formattedText = new FormattedText(
                textToDraw,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily),
                FontSize * scale,
                Color,
                1.0);

            dc.DrawText(formattedText, new Point(X, Y));
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
