using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
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
        public ImageSource Source { get; private set; }
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

            var dpiScale = 96.0 / dpi;
            
            Rect rect = new Rect(
                X * dpiScale,
                Y * dpiScale,
                Width * dpiScale,
                Height * dpiScale);
            
            dc.DrawImage(Source, rect);
        }

        public override string GetTspl()
        {
            var (widthBytes, height, hexData) = ConvertToTsplBitmap(Path,  Width, Height);
            return $"BITMAP {X},{Y},{widthBytes},{height},0,{hexData}";
        }

        private static (int widthBytes, int height, string hex) ConvertToTsplBitmap(string path, int targetWidth,  int targetHeight)
        {
            using var source = new Bitmap(path);
            
            Bitmap resized = new Bitmap(source, new Size(targetWidth, targetHeight));
            
            Bitmap bw = ConvertTo1Bit(resized);
            
            int widthBytes = (bw.Width + 7) / 8;
            int height = bw.Height;
            
            StringBuilder sb = new StringBuilder(widthBytes * height * 2);

            for (int y = 0; y < height; y++)
            {
                byte current = 0;
                int bits = 0;

                for (int x = 0; x < bw.Width; x++)
                {
                    var pixel = bw.GetPixel(x, y);
                    bool black = pixel.R < 128;

                    current <<= 1;
                    if (black) current |= 1;
                    bits++;

                    if (bits == 8)
                    {
                        sb.Append(current.ToString("X2"));
                        current = 0;
                        bits = 0;
                    }
                }

                if (bits > 0)
                {
                    current <<= (8 - bits);
                    sb.Append(current.ToString("X2"));
                }
            }
            return (widthBytes, height, sb.ToString());
        }

        private static Bitmap ConvertTo1Bit(Bitmap src)
        {
            // Step 1. Draw original image to a 32-bit buffer
            Bitmap temp = new Bitmap(src.Width, src.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(temp))
            {
                g.DrawImage(src, new Rectangle(0, 0, src.Width, src.Height));
            }

            // Step 2. Create a 1-bit output bitmap
            Bitmap bw = new Bitmap(src.Width, src.Height, PixelFormat.Format1bppIndexed);

            // Lock bits for fast pixel access
            BitmapData data = bw.LockBits(
                new Rectangle(0, 0, bw.Width, bw.Height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format1bppIndexed);

            for (int y = 0; y < src.Height; y++)
            {
                byte[] scan = new byte[(src.Width + 7) / 8];
                int bitIndex = 0;
                byte currentByte = 0;

                for (int x = 0; x < src.Width; x++)
                {
                    Color pixel = temp.GetPixel(x, y);

                    bool black = (pixel.R + pixel.G + pixel.B) / 3 < 128;

                    currentByte <<= 1;
                    if (black) currentByte |= 1;

                    bitIndex++;

                    if (bitIndex == 8)
                    {
                        scan[(x / 8)] = currentByte;
                        currentByte = 0;
                        bitIndex = 0;
                    }
                }

                if (bitIndex > 0)
                {
                    currentByte <<= (8 - bitIndex);
                    scan[src.Width / 8] = currentByte;
                }

                // Copy row to output
                System.Runtime.InteropServices.Marshal.Copy(scan, 0, data.Scan0 + data.Stride * y, scan.Length);
            }

            bw.UnlockBits(data);
            temp.Dispose();

            return bw;
        }


        public void UpdatePath(string newPath)
        {
            Path = newPath;
        }
    }
}
