using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using DayOfWeek = System.DayOfWeek;

namespace Willhaben.Domain.Utils.Converters;

public class DayOfWeekJsonConverter : JsonConverter<List<DayOfWeek>>
{
    public override List<DayOfWeek> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        var days = new List<DayOfWeek>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return days;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string day = reader.GetString();
                if (Enum.TryParse<DayOfWeek>(day, true, out var dayEnum))
                {
                    if (days.Any(d => d.Equals(dayEnum)))
                    {
                        throw new EnumKeyExistsException<DayOfWeek>(dayEnum);
                    }

                    days.Add(dayEnum);
                }
                else
                {
                    throw new StringEnumConversionException(day ?? "day");
                }
            }
        }

        throw new JsonException();
    }
    public override void Write(Utf8JsonWriter writer, List<DayOfWeek> days, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var dayOfWeek in days)
        {
            writer.WriteStringValue(dayOfWeek.ToString());
        }
        writer.WriteEndArray();
    }
}