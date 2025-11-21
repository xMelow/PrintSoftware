using PrintSoftware.Domain.Label;
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
using PrintSoftware.Domain;

namespace PrintSoftware.Services
{
    public class LabelService
    {
        private readonly string _labelsPath = Path.Combine(AppContext.BaseDirectory, "Labels");
        public Label CurrentLabel { get; set; }

        public LabelService() { }

        public Label GetLabel(string labelName)
        {
            return GetLabelFromJson(labelName);
        }

        public List<Label> GetAllLabels()
        {
            var labelsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Labels");

            if (!Directory.Exists(labelsFolder))
                throw new DirectoryNotFoundException($"Labels folder {labelsFolder} not found");

            var labelFiles = Directory.GetFiles(labelsFolder)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
            
            var labels = ConvertFileNamesToLabels(labelFiles);
            
            return labels;
        }

        private List<Label> ConvertFileNamesToLabels(List<string> labelFiles)
        {
            var result = new List<Label>();
            
            foreach (var labelFile in labelFiles)
            {
                var label = GetLabel(labelFile);
                result.Add(label);
            }
            return result;
        }

        private Label GetLabelFromJson(string name)
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

        private void SaveLabelToJson(Label label)
        {
            var filePath = Path.Combine(_labelsPath, $"{label.Name}.json");

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters = { new LabelElementJsonConverter() }
            };
            
            string json = JsonSerializer.Serialize(label, options);
            File.WriteAllText(filePath, json);
        }
        
        public void UpdateLabelDataElement(string name, string data)
        {
            CurrentLabel.UpdateLabelData(name, data);
        }

        public Label UpdateLabelData(Dictionary<string, string> labelData)
        {
            CurrentLabel.UpdateLabelData(labelData);
            return CurrentLabel;
        }

        public Label UpdateLabelDataFromRow(DataRow row)
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
