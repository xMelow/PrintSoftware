using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PrintSoftware.Interfaces;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;
using Brushes = System.Windows.Media.Brushes;
using FormatException = System.FormatException;
using Point = System.Windows.Point;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class BarcodeElement : LabelElement, IDynamicElement
    {
        public string Content { get; set; }
        public string? Name { get; set; }
        public string CodeType { get; set; }
        public int Height { get; set; }
        public int HumanRead { get; set; }
        public int Rotation { get; set; }
        public int Narrow { get; set; }
        public int Wide { get; set; }

        public BarcodeElement()
        {
            Type = "BARCODE";
        }

        public BarcodeElement(string name, string codeType, int height, int humanRead, int rotation, int narrow, int wide, string content)
        {
            Type = "BARCODE";
            Name = name;
            CodeType = codeType;
            Height = height;
            HumanRead = humanRead;
            Rotation = rotation;
            Narrow = narrow;
            Wide = wide;
            Content = content;
        }

        public override void Draw(DrawingContext dc, int dpi)
        {
            if (string.IsNullOrEmpty(Content))
                return;

            var barcodeFormat = GetBarcodeType();

            int pixelWidth = (int)Math.Max(X, 10);
            int pixelHeight = (int)Math.Max(Y, 10);

            var writer = new BarcodeWriter
            {
                Format = barcodeFormat,
                Options = new EncodingOptions
                {
                    Height = pixelHeight,
                    Width = pixelWidth,
                    PureBarcode = HumanRead == 1,
                    Margin = 0
                },
                Renderer = new BitmapRenderer()
            };

            Bitmap bitmap = writer.Write(Content);
            BitmapImage barcodeImage = ToBitmapImage(bitmap);

            double cx = X + pixelWidth / 2.0;
            double cy = Y + Height / 2.0;

            dc.PushTransform(new RotateTransform(Rotation, cx, cy));

            dc.DrawImage(barcodeImage, new Rect(X, Y, pixelWidth, Height));

            if (HumanRead == 2)
            {
                var text = new FormattedText(
                    Content,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Arial"),
                    10,
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(Application.Current.MainWindow).PixelsPerDip);

                dc.DrawText(text, new Point(X, Y + Height + 2));
            }
            dc.Pop();
        }

        private BarcodeFormat GetBarcodeType()
        {
            BarcodeFormat format = CodeType switch
            {
                "128" => BarcodeFormat.CODE_128,
                "128M" => BarcodeFormat.CODE_128,
                "EAN13" => BarcodeFormat.EAN_13,
                _ => BarcodeFormat.CODE_128
            };
            return format;
        }
        
        private static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using var memory = new MemoryStream();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
            memory.Position = 0;
            
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = memory;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            
            return bitmapImage;
        }
        
        public override string GetTspl()
        { 
            return $"{Type} {X},{Y},\"{CodeType}\",{Height},{HumanRead},{Rotation},{Narrow},{Wide},\"{Content}\"";
        }

        public void UpdateContent(string newValue)
        {
            Content = newValue;
        }
    }
}
