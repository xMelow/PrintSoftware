using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PrintSoftware.Domain.Label;
using PrintSoftware.Domain.Label.LabelElements;

namespace PrintSoftware.Domain.Label
{
    public class LabelElementJsonConverter : JsonConverter<LabelElement>
    {
        public override LabelElement? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
            {
                var root = document.RootElement;

                if (!root.TryGetProperty("Type", out JsonElement typeProperty))
                    throw new JsonException("Missing type property in LabelElement");

                string? type = typeProperty.GetString();

                LabelElement? element = type switch
                {
                    "TEXT" => MapTextElement(root, options),
                    "QRCODE" => root.Deserialize<QRCodeElement>(options),
                    "BAR" => root.Deserialize<BarElement>(options),
                    "BOX" => root.Deserialize<BoxElement>(options),
                    "CIRCLE" => root.Deserialize<CircleElement>(options),
                    "BARCODE" => root.Deserialize<BarcodeElement>(options),
                    "IMAGE" => root.Deserialize<ImageElement>(options),
                    "COUNTER" => root.Deserialize<CounterElement>(options),
                    "BLOCK" => root.Deserialize<TextBlockElement>(options),
                    _ => throw new JsonException($"Unkown element type: {type}")
                };
                return element;
            }
        }
        
        private TextElement MapTextElement(JsonElement root, JsonSerializerOptions options)
        {
            var textElem = root.Deserialize<TextElement>(options) ?? new TextElement();
            
            if (root.TryGetProperty("Name", out var varProp))
                textElem.Name = varProp.ValueKind == JsonValueKind.Null ? null : varProp.GetString();
        
            return textElem;
        }

        public override void Write(Utf8JsonWriter writer, LabelElement value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options); 
        }
    }
}
