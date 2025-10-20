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
                if (!document.RootElement.TryGetProperty("Type", out JsonElement typeProperty))
                    throw new JsonException("Missing type property in LabelElement");

                string type = typeProperty.GetString();

                LabelElement element = type switch
                {
                    "Text" => document.RootElement.Deserialize<TextElement>(options),
                    "QRcode" => document.RootElement.Deserialize<QRCodeElement>(options),
                    "Bar" => document.RootElement.Deserialize<LineElement>(options),
                    "Box" => document.RootElement.Deserialize<RectangleElement>(options),
                    _ => throw new JsonException($"Unkown element type: {type}")
                };

                return element;
            }
        }

        public override void Write(Utf8JsonWriter writer, LabelElement value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options); 
        }
    }
}
