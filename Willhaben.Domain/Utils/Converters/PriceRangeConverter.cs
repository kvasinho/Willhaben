using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters;

public class PriceRangeConverter : JsonConverter<PriceRange>
{
    public override PriceRange Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;
            var priceRange = new PriceRange
            {
                PriceFrom = root.GetProperty("PriceFrom").GetInt32(),
                PriceTo = root.GetProperty("PriceTo").GetInt32()
            };
            return priceRange;
        }
    }

    public override void Write(Utf8JsonWriter writer, PriceRange value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("PriceFrom", value.PriceFrom ?? 0);
        writer.WriteNumber("PriceTo", value.PriceTo ?? 0);
        writer.WriteEndObject();
    }
}