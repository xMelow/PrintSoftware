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
        public string Content { get; set; }

        public override void Draw(DrawingContext dc, double scale)
        {
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
                double widthPx = (Width > 0 ? Width : bitmap.Width) * scale;
                double heightPx = (Height > 0 ? Height : bitmap.Height) * scale;

                //Debug.WriteLine($"QRCode: X={X} y={Y} width={Width} height={Height}");

                dc.DrawImage(bitmapSource, new Rect(xPx, yPx, widthPx, heightPx));
            }
        }

        public override string CreateTspl()
        {
            return $"QRCODE {X},{Y},L,14,A,0,M2,S7,\"{Content}\"";
        }

        public override void UpdateContent(string newValue)
        {
            Content = newValue;
        }
    }
}
