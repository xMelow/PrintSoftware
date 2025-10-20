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
            _currentLabel = LoadLabel("TestLabel");`
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

        public void SaveLabel(Label label)
        {
            // implement
        }

        public Label GetLabel() => _currentLabel;

        //public Label CreateLabel(Dictionary<string, string> labelData)
        //{
        //    _currentLabel = new Label(labelData);
        //    return _currentLabel;
        //}

        public Label CreateLabelWithElements(string title, string name, string phone, string email, string company)
        {
            //_currentLabel = new Label
            //                {
            //                    Width = 110,
            //                    Height = 110,
            //                    Elements = new List<LabelElement>
            //                    {
            //                        new TextElement { X = 10, Y = 10, Text = title, FontSize = 12 },
            //                        new TextElement { X = 10, Y = 40, Text = "Name:", FontSize = 10 },
            //                        new TextElement { X = 10, Y = 60, Text = "Phone number:", FontSize = 10 },
            //                        new TextElement { X = 10, Y = 80, Text = "Email:", FontSize = 10 },
            //                        new TextElement { X = 10, Y = 100, Text = "Company:", FontSize = 10 },
            //                        new TextElement { X = 45, Y = 40, Text = name, FontSize = 10 },
            //                        new TextElement { X = 80, Y = 60, Text = phone, FontSize = 10 },
            //                        new TextElement { X = 40, Y = 80, Text = email, FontSize = 10 },
            //                        new TextElement { X = 60, Y = 100, Text = company, FontSize = 10 },
            //                        new LineElement { X = 33, Y = 135, Width = 1199, Height = 7 },
            //                        new RectangleElement { X = 33, Y = 178, Width = 1199, Height = 712 },
            //                        new CircleElement { X = 933, Y = 590, Radius = 260 },
            //                    }
            //                };
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
