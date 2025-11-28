using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PrintSoftware.Interfaces;
using QRCoder;


namespace PrintSoftware.Domain.Label.LabelElements
{
    public class QRCodeElement : LabelElement, IDynamicElement
    {
        public string Name { get; set; }
        public string ECCLevel { get; set; }
        public int CellWidth { get; set; }
        public string Mode { get; set; }
        public int Rotation { get; set; }
        public string Model { get; set; }
        public string Mask { get; set; }
        public string Content { get; set; }

        public QRCodeElement()
        {
            Type = "QRCODE";
        }
        
        public QRCodeElement(string name, int x, int y, string eccLevel, int cellWidth, string mode, int rotation, string model, string mask, string content)
        {
            Type = "QRCODE";
            Name = name;
            X = x;
            Y = y;
            ECCLevel = eccLevel;
            CellWidth = cellWidth;
            Mode = mode;
            Rotation = rotation;
            Model = model;
            Mask = mask;
            Content = content;
        }
        
        public QRCodeElement(string name, int x, int y, string content)
        {
            Type = "QRCODE";
            Name = name;
            X = x;
            Y = y;
            ECCLevel = "L";
            CellWidth = 5;
            Mode = "A";
            Rotation = 0;
            Model = "M2";
            Mask = "S7";
            Content = content;
        }
        public override void Draw(DrawingContext dc, int dpi)
        {
            // using (var qrGenerator = new QRCodeGenerator())
            // using (var data = qrGenerator.CreateQrCode(Content, QRCodeGenerator.ECCLevel.Q))
            // using (var qrCode = new QRCode(data))
            // using (var bitmap = qrCode.GetGraphic(20))
            // {
            //     var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //         bitmap.GetHbitmap(),
            //         IntPtr.Zero,
            //         Int32Rect.Empty,
            //         BitmapSizeOptions.FromWidthAndHeight(
            //             (int)(bitmap.Width * dpi),
            //             (int)(bitmap.Height * dpi))
            //     );
            //     
            //     double widthPx = (X > 0 ? X : bitmap.Width) * dpi;
            //     double heightPx = (Y > 0 ? Y : bitmap.Height) * dpi;
            //
            //     //Debug.WriteLine($"QRCode: X={X} y={Y} width={Width} height={Height}");
            //
            //     dc.DrawImage(bitmapSource, new Rect(X, Y, widthPx, heightPx));
            // }
        }

        public override string GetTspl()
        {
            return $"{Type} {X},{Y},{ECCLevel},{CellWidth},{Mode},{Rotation},{Model},{Mask},\"{Content}\"";
        }

        public void UpdateContent(string newContent)
        {
            Content = newContent;
        }
    }
}
