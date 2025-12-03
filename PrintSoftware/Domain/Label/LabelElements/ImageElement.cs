using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Size = System.Drawing.Size;

namespace PrintSoftware.Domain.Label.LabelElements
{
    public class ImageElement : LabelElement
    {
        public ImageSource Source { get; }
        public string Path { get; private set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ImageElement(string path, int width, int height)
        {
            Type = "IMAGE";
            Path = path;
            Width = width;
            Height = height;
            Source = new BitmapImage(new Uri(path));
        }

        public override void Draw(DrawingContext dc, int dpi)
        {
            if (Source == null)
                return;
            
            Rect rect = new Rect(
                X,
                Y,
                Width,
                Height);
            
            dc.DrawImage(Source, rect);
        }

        public override string GetTspl()
        {
            var hexData = ConvertImageToBitmapHexadata();
            return $"BITMAP {X},{Y},{Width},{Height},0,{hexData}";
        }

        private string ConvertImageToBitmapHexadata()
        {
            Bitmap bitmap = new Bitmap(Path);
            Bitmap bwBitmap = ConvertToBlackAndWhite(bitmap);
            string hexData = BitmapToHex(bwBitmap);
            
            return hexData;
        }

        private Bitmap ConvertToBlackAndWhite(Bitmap original)
        {
            Bitmap bw = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color pixel = original.GetPixel(x, y);
                    bool black = (pixel.R + pixel.G + pixel.B) / 3 < 128;
                    bw.SetPixel(x, y, black ? Color.Black : Color.White);
                }
            }
            return bw;
        }

        private string BitmapToHex(Bitmap bitmap)
        {
            StringBuilder hexData = new StringBuilder();
            int widthBytes = (bitmap.Width + 7) / 8;
            
            for (int row = 0; row < bitmap.Height; row++)
            {
                for (int b = 0; b < widthBytes; b++)
                {
                    byte data = 0;
                    for (int bit = 0; bit < 8; bit++)
                    {
                        int col = b * 8 + bit;
                        if (col < bitmap.Width)
                        {
                            Color pixel = bitmap.GetPixel(col, row);
                            int brightness = (pixel.R + pixel.G + pixel.B) / 3;
                            bool isBlack = brightness < 128;
                            if (isBlack)
                                data |= (byte)(1 << (7 - bit));
                        }
                    }
                    hexData.Append(data.ToString("X2"));
                }
            }
            return hexData.ToString();
        }
        
        public void UpdatePath(string newPath)
        {
            Path = newPath;
        }
    }
}
