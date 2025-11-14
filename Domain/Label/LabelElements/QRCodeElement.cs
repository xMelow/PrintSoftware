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
            Type = "QRCode";
        }
        
        public QRCodeElement(string name, int x, int y, string eccLevel, int cellWidth, string mode, int rotation, string model, string mask, string content)
        {
            Type = "QRCode";
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
        

        public override void Draw(DrawingContext dc, double scale)
        {
            //TODO: FIX draw function QRCode not showing correct in the label preview
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

        public void UpdateContent(string newContent)
        {
            Content = newContent;
        }
    }
}
