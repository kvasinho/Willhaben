using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

public class IntervalJsonConverter : JsonConverter<Interval>
{
    public override Interval Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string value = reader.GetString();
        return value switch
        {
            "M1" => Interval.M1,
            "M2" => Interval.M2,
            "M5" => Interval.M5,
            "M10" => Interval.M10,
            "M15" => Interval.M15,
            "M30" => Interval.M30,
            "H1" => Interval.H1,
            "H2" => Interval.H2,
            "H4" => Interval.H4,
            "H6" => Interval.H6,
            "H12" => Interval.H12,
            _ =>  throw new StringEnumConversionException(value)
        };
    }

    public override void Write(Utf8JsonWriter writer, Interval value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}