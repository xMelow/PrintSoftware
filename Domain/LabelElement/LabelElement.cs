using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SimpleProject.Domain.Labels
{
    public abstract class LabelElement
    {
        protected string Type { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
       
        public abstract void Draw(DrawingContext dc, double scale);

        public abstract string GetTspl();
    }
}
