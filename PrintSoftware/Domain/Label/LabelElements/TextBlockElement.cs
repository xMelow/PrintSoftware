using System.Globalization;
using System.Windows;
using System.Windows.Media;
using PrintSoftware.Interfaces;

namespace PrintSoftware.Domain.Label.LabelElements;

public class TextBlockElement : LabelElement, IDynamicElement
{
    public string Name { get; set; }
    public string Content { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string FontFamily { get; set; }
    public int Rotation { get; set; }
    public double FontSize { get; set; }
    public int Fit { get; set; }
    public Brush Color { get; set; } = Brushes.Black;

    public TextBlockElement(string name, string content, int width, int height, int rotation, string fontFamily, double fontSize)
    {
        Type = "BLOCK";
        Name = name;
        Content = content;
        Width = width;
        Height = height;
        Rotation = rotation;
        FontFamily = fontFamily;
        FontSize = fontSize;
    }
    
    public override void Draw(DrawingContext dc, int dpi)
    {
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

    public override string GetTspl()
    {
        return $"BLOCK {X},{Y},{Width},{Height},\"{FontFamily}\",{Rotation},{FontSize},{FontSize},\"{Content}\"";
    }
    public void UpdateContent(string newContent)
    {
        Content = newContent;
    }
}