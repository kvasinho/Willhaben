using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters;

public class UtcTimeJsonConverter : JsonConverter<UTCTime>
{
    public override UTCTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string timeString = reader.GetString() ?? throw new JsonException("Expected a string.");

        TimeOnly timeOnly = TimeOnly.Parse(timeString, null);

        return new UTCTime(timeOnly, isUtc: true);
    }

    public override void Write(Utf8JsonWriter writer, UTCTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.UtcTime.ToString("HH:mm"));
    }
}