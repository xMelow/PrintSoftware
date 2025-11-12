using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using QRCoder;


namespace SimpleProject.Domain.Labels
{
    public class QRCodeElement : LabelElement
    {
        private string Name { get; set; }
        private string ECCLevel { get; set; }
        private int CellWidth { get; set; }
        private string Mode { get; set; }
        private int Rotation { get; set; }
        private string Model { get; set; }
        private string Mask { get; set; }
        public string Content { get; set; }

        public QRCodeElement(string name, int x, int y, string eccLevel, int cellWidth, string mode, int rotation, string model, string mask, string content)
        {
            Type = "QRCode";
            Name = name;
            ECCLevel = eccLevel;
            CellWidth = cellWidth;
            Mode = mode;
            Rotation = rotation;
            Model = model;
            Mask = mask;
            Content = content;
        }
        

        public override void Draw(DrawingContext dc, double scale)
        {
            //TODO: FIX draw function QRCode not showing in the label preview
            using (var qrGenerator = new QRCodeGenerator())
            using (var data = qrGenerator.CreateQrCode(Content, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(data))
            using (var bitmap = qrCode.GetGraphic(20))
            {
                var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmap.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(
                        (int)(bitmap.Width * scale),
                        (int)(bitmap.Height * scale))
                );

                double xPx = X * scale;
                double yPx = Y * scale;
                double widthPx = (X > 0 ? X : bitmap.Width) * scale;
                double heightPx = (Y > 0 ? Y : bitmap.Height) * scale;

                //Debug.WriteLine($"QRCode: X={X} y={Y} width={Width} height={Height}");

                dc.DrawImage(bitmapSource, new Rect(xPx, yPx, widthPx, heightPx));
            }
        }

        public override string GetTspl()
        {
            return $"{Type} {X},{Y},L,14,A,0,M2,S7,\"{Content}\"";
        }

        public void UpdateQrCodeContent(string newValue)
        {
            Content = newValue;
        }
    }
}
