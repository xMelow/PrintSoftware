using System;
using System.Collections.Generic;
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

        public override void Draw(DrawingContext dc, double scale, double dpi)
        {
            if (string.IsNullOrEmpty(Content))
                return;

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

                dc.DrawImage(bitmapSource, new Rect(X, Y, Width * scale, Height * scale));
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
