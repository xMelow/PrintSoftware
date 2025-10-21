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
            Debug.WriteLine($"Label '{_currentLabel.Name}' has {_currentLabel.LabelElements.Count} elements.");

            var dv = new DrawingVisual();

            using (var dc = dv.RenderOpen())
            {
                _currentLabel.Draw(dc, visibilityScale);
            }

            var bmp = new RenderTargetBitmap(
                 (int)(500 * visibilityScale),
                 (int)(700 * visibilityScale),
                 96 * visibilityScale,
                 96 * visibilityScale,
                 PixelFormats.Pbgra32);

            bmp.Render(dv);
            return bmp;
        }
    }
}
