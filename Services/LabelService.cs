using SimpleProject.Domain;
using SimpleProject.Domain.Labels;
using System;
using System.Collections.Generic;
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

        public BitmapSource RenderLabelPreview(int visibilityScale = 1)
        {
            const double mmPerInch = 25.4;
            const double dpi = 300;
            double pixelWidth = (_currentLabel.Width / mmPerInch) * dpi * visibilityScale;
            double pixelHeight = (_currentLabel.Height / mmPerInch) * dpi * visibilityScale;

            if (pixelWidth <= 0 || pixelHeight <= 0)
                throw new InvalidOperationException($"Invalid label size: {pixelWidth}x{pixelHeight}");

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                _currentLabel.Draw(dc, visibilityScale, dpi);
            }

            var bmp = new RenderTargetBitmap(
                 (int)(pixelWidth * visibilityScale),
                 (int)(pixelHeight * visibilityScale),
                 dpi,
                 dpi,
                 PixelFormats.Pbgra32);

            bmp.Render(dv);
            return bmp;
        }
    }
}
