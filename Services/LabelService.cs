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
        private readonly string _labelsPath;
        private readonly Label _currentLabel;

        public LabelService()
        {
            _labelsPath = Path.Combine(AppContext.BaseDirectory, "Labels");
            _currentLabel = LoadLabel("TestLabel");
        }

        private Label LoadLabel(string name)
        {
            var filePath = Path.Combine(_labelsPath, $"{name}.json");

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

        public Label UpdateLabelData(string fieldTag, string fieldData)
        {
            _currentLabel.UpdateLabelData(fieldTag, fieldData);
            return _currentLabel;
        }

        public Label UpdateLabelData(Dictionary<string, string> labelData)
        {
            _currentLabel.UpdateLabelData(labelData);
            return _currentLabel;
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
