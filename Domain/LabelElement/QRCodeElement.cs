using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using QRCoder;


namespace SimpleProject.Domain.Labels
{
    public class QRCodeElement : LabelElement
    {
        public string Content { get; set; }

        public override void Draw(DrawingContext dc, double scale)
        {
            throw new NotImplementedException();
            // implement QRCode library QRCoder
            //using (var qrGenerator = new QRCodeGenerator())
            //{
            //    var data = qrGenerator.CreateQrCode(Content, QRCodeGenerator.ECCLevel.Q);
            //    var qrCode = new QRCode(data);
            //    using (var bitmap = qrCode.GetGraphic(20))
            //    {
            //        var bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //            bitmap.GetHbitmap(),
            //            IntPtr.Zero,
            //            System.Windows.Int32Rect.Empty,
            //            BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height)
            //        );
            //        dc.DrawImage(bitmapSource, new Rect(X, Y, Size, Size));
            //    }
            //}
        }

        public override string CreateTspl()
        {
            return $"QRCODE {X},{Y},L,14,A,0,M2,S7,\"{Content}\"";
        }
    }
}
