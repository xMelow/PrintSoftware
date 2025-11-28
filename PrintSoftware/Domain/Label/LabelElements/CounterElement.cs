using System.Globalization;
using System.Windows;
using System.Windows.Media;
using PrintSoftware.Interfaces;

namespace PrintSoftware.Domain.Label.LabelElements;

public class CounterElement : LabelElement, IDynamicElement
{

    public string Name { get; set; } = "0";
    public string Content { get; set; }
    public int StartValue { get; set; } = 1;
    public int CounterStep { get; set; } = 1;
    public string FontFamily { get; set; }
    public int Rotation { get; set; }
    public double FontSize { get; set; }
    public Brush Color { get; set; } = Brushes.Black;
    
    public CounterElement()
    {
        Type = "TEXT";
    }

    public CounterElement(string name, int x, int y,  string fontFamily, int rotation, double fontSize, string content)
    {
        Type = "TEXT";
        Name = name;
        Content = content;
        X = x;
        Y = y;
        FontFamily = fontFamily;
        FontSize = fontSize;
        Rotation = rotation;
    }

    public void UpdateContent(string newContent)
    {
        SetStartingValue(newContent);
        Content = newContent; 
    }

    private void SetStartingValue(string newContent)
    {
        StartValue = int.Parse(newContent);
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
        string tspl = $"SET COUNTER @{Name} +{CounterStep} \r\n";
        tspl += $"@{Name}=\"{StartValue}\" \r\n";
        tspl += $"{Type} {X},{Y},\"{FontFamily}\",{Rotation},{FontSize},{FontSize},@{Name}";
        
        return tspl;
    }
}