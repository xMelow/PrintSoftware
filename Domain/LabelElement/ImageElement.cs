using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace SimpleProject.Domain.Labels
{
    public class ImageElement : LabelElement
    {
        public ImageSource Source { get; set; }
        public string Path { get; set; } = "";

        public override void Draw(DrawingContext dc, double scale)
        {
            if (Source != null)
            {
                dc.DrawImage(Source, new Rect(X, Y, Width, Height));
            }
        }

        public override string CreateTspl()
        {
            throw new NotImplementedException();
        }

        public override void UpdateContent(string newValue)
        {
            Path = newValue;
        }
    }
}
