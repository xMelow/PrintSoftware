using SimpleProject.Domain.Labels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleProject.Domain
{
    public class LabelElementJsonConverter : JsonConverter<LabelElement>
    {
        public override LabelElement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                if (!document.RootElement.TryGetProperty("Type", out JsonElement typeProperty))
                    throw new JsonException("Missing type property in LabelElement");

                string? type = typeProperty.GetString();

                LabelElement? element = type switch
                {
                    "Text" => MapTextElement(root, options),
                    "QRcode" => document.RootElement.Deserialize<QRCodeElement>(options),
                    "Bar" => document.RootElement.Deserialize<LineElement>(options),
                    "Box" => document.RootElement.Deserialize<RectangleElement>(options),
                    "Circle" => document.RootElement.Deserialize<CircleElement>(options),
                    _ => throw new JsonException($"Unkown element type: {type}")
                };

                return element;
            }
        }

        private TextElement MapTextElement(JsonElement root, JsonSerializerOptions options)
        {
            var textElem = root.Deserialize<TextElement>(options) ?? new TextElement();

            if (root.TryGetProperty("Content", out var contentProp))
                textElem.Text = contentProp.ValueKind == JsonValueKind.Null ? null : contentProp.GetString();
            if (root.TryGetProperty("VariableName", out var varProp))
                textElem.VariableName = varProp.ValueKind == JsonValueKind.Null ? null : varProp.GetString();

            return textElem;
        }

        public override void Write(Utf8JsonWriter writer, LabelElement value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options); 
        }
    }
}
