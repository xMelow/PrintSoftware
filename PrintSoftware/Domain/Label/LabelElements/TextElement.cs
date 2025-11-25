using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using PrintSoftware.Interfaces;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class TextElement : LabelElement, IDynamicElement
    {
        public string? Name { get; set; }
        public string Content { get; set; }
        public string FontFamily { get; set; }
        public int Rotation { get; set; }
        public double FontSize { get; set; }
        public Brush Color { get; set; } = Brushes.Black;

        public TextElement()
        {
            Type = "TEXT";
        }

        public TextElement(int x, int y, string fontFamily, int rotation ,double fontSize, string content)
        {
            Type = "TEXT";
            X = x;
            Y = y;
            FontFamily = fontFamily;
            Rotation = rotation;
            FontSize = fontSize;
            Content = content;
        }
        
        public TextElement(string name, int x, int y, string content, string fontFamily, double fontSize)
        {
            Type = "TEXT";
            Name = name;
            X = x;
            Y = y;
            FontFamily = fontFamily;
            FontSize = fontSize;
            Content = content;
        }

        public override void Draw(DrawingContext dc, int dpi)
        {
            double scale = dpi / 96.0;
            
            var typeface = new Typeface(
                new FontFamily("Arial"),
                FontStyles.Normal,
                FontWeights.Normal,
                FontStretches.Normal
            );

            var formattedText = new FormattedText(
                Content,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                FontSize,
                Color,
                dpi);

            dc.DrawText(formattedText, new Point(X, Y));
        }
        
        //TEXT 77,505,""0"",0,16,16,""Company:""
        //TEXT x,y, « font « ,rotation,x-multiplication,y-multiplication,[alignment,] « content «
        public override string GetTspl()
        {
            var content = Content ?? string.Empty;
            return $"{Type} {X},{Y},\"0\",0,{FontSize},{FontSize},\"{content}\"";
        }

        public void UpdateContent(string content)
        {
            Content = content;
        }
    }
}
