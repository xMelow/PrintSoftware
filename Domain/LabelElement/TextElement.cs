using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using SimpleProject.Interfaces;

namespace SimpleProject.Domain.Labels
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

        public override void Draw(DrawingContext dc, double scale)
        {
            if (string.IsNullOrEmpty(Content))
                return;

            double xPx = X * scale;
            double yPx = Y * scale;

            //Debug.WriteLine($"Text: X={X}, Y={Y}, W={Width}, H={Height}, xPx={xPx}, yPx={yPx}");

            var typeface = new Typeface(
                    new FontFamily(FontFamily),
                    FontStyles.Normal,
                    FontWeights.Bold,
                    FontStretches.Normal
                );

            var formattedText = new FormattedText(
                Content,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                FontSize * 40,
                Color,
                96);

            dc.PushTransform(new TranslateTransform(xPx, yPx));
            dc.DrawText(formattedText, new Point(0, 0));
            dc.Pop();
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
