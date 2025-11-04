using SimpleProject.Domain;
using SimpleProject.Domain.Labels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace SimpleProject.Services
{
    public class LabelService
    {
        private readonly string labelsPath;

        private Label _currentLabel;

        public LabelService()
        {
            labelsPath = Path.Combine(AppContext.BaseDirectory, "Labels");
            _currentLabel = LoadLabel("TestLabel");
        }

        public Label LoadLabel(string name)
        {
            var filePath = Path.Combine(labelsPath, $"{name}.json");
            Debug.WriteLine(filePath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Label {name} not found");

            string json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                Converters = { new LabelElementJsonConverter() }
            };
            return JsonSerializer.Deserialize<Label>(json, options);
        }

        public Label GetLabel() => _currentLabel;

        public Label UpdateLabelData(Dictionary<string, string> labelData)
        {
            _currentLabel.UpdateLabelData(labelData);
            return _currentLabel;
        }

        public BitmapSource RenderLabelPreview()
        {
            int scale = 10;
            int dpi = 96;

            double maxX = _currentLabel.LabelElements.Max(e => e.Xend > 0 ? e.Xend : e.X);
            double maxY = _currentLabel.LabelElements.Max(e => e.Yend > 0 ? e.Yend : e.Y);

            double pixelWidth = maxX * scale;
            double pixelHeight = maxY * scale;

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                _currentLabel.Draw(dc, scale);
            }

            var bmp = new RenderTargetBitmap(
                 (int)pixelWidth,
                 (int)pixelHeight,
                 dpi,
                 dpi,
                 PixelFormats.Pbgra32);

            bmp.Render(dv);
            return bmp;
        }

        public Label CreateLabelFromRow(DataRow row)
        {
            var labelData = new Dictionary<string, string>
                {
                    { "Title", row["ID"].ToString() },
                    { "Name", row["NAME"].ToString() },
                    { "PhoneNumber", row["PHONENUMBER"].ToString() },
                    { "Email", row["EMAIL"].ToString() },
                    { "Company", row["OCCUPATION"].ToString() },
                    { "QR", row["POSTCODE"].ToString() }
                };

            return UpdateLabelData(labelData);
        }
    }
}
