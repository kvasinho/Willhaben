using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters;
public class DayOfWeekCollectionConverter : JsonConverter<DayOfWeekCollection>
    {
        public override DayOfWeekCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of array.");
            }

            var days = new List<DayOfWeek>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    var collection = new DayOfWeekCollection();
                    collection.SetSimplifiedValues(days);
                    return collection;
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    if (reader.TryGetInt32(out int dayOfWeekValue))
                    {
                        if (Enum.IsDefined(typeof(DayOfWeek), dayOfWeekValue))
                        {
                            var day = (DayOfWeek)dayOfWeekValue;
                            if (days.Contains(day))
                            {
                                throw new EnumKeyExistsException<DayOfWeek>(day);
                            }
                            days.Add(day);
                        }
                        else
                        {
                            throw new StringEnumConversionException($"Invalid DayOfWeek value: {dayOfWeekValue}");
                        }
                    }
                    else
                    {
                        throw new JsonException($"Expected integer value for DayOFWeek.");
                    }
                }
                else
                {
                    throw new JsonException($"Unexpected token type: {reader.TokenType}");
                }
            }

            throw new JsonException("Unexpected end of array.");
        }

        public override void Write(Utf8JsonWriter writer, DayOfWeekCollection dayOfWeekCollection, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var dayOfWeek in dayOfWeekCollection.SimplifiedValues)
            {
                writer.WriteNumberValue((int)dayOfWeek);
            }
            writer.WriteEndArray();
        }
    }