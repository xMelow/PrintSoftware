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
            throw new NotImplementedException();
        }

        public override string GetTspl()
        {
            throw new NotImplementedException();
        }

        public void UpdatePath(string newValue)
        {
            Path = newValue;
        }
    }
}
