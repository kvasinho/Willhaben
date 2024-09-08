using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters;


public class LocationCollectionConverter : JsonConverter<LocationCollection>
    {
        public override LocationCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of array.");
            }

            var locations = new List<Location>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    var collection = new LocationCollection();
                    collection.SetSimplifiedValues(locations);
                    return collection;
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    if (reader.TryGetInt32(out int locationValue))
                    {
                        if (Enum.IsDefined(typeof(Location), locationValue))
                        {
                            var location = (Location)locationValue;
                            if (locations.Contains(location))
                            {
                                throw new EnumKeyExistsException<Location>(location);
                            }
                            locations.Add(location);
                        }
                        else
                        {
                            throw new StringEnumConversionException($"Invalid Location value: {locationValue}");
                        }
                    }
                    else
                    {
                        throw new JsonException($"Expected integer value for Location.");
                    }
                }
                else
                {
                    throw new JsonException($"Unexpected token type: {reader.TokenType}");
                }
            }

            throw new JsonException("Unexpected end of array.");
        }

        public override void Write(Utf8JsonWriter writer, LocationCollection locationCollection, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var location in locationCollection.SimplifiedValues)
            {
                writer.WriteNumberValue((int)location);
            }
            writer.WriteEndArray();
        }
    }
public class SimplifyableCollectionConverter<TCollection, TEnum> : JsonConverter<TCollection>
    where TCollection : SimplifyableEnumCollection<TEnum>, new()
    where TEnum : Enum
{
    public override TCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected start of array.");
        }

        var values = new List<TEnum>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                var collection = new TCollection();
                collection.SetSimplifiedValues(values);
                return collection;
            }

            if (reader.TokenType == JsonTokenType.Number)
            {
                if (reader.TryGetInt32(out int value))
                {
                    if (Enum.IsDefined(typeof(TEnum), value))
                    {
                        var enumValue = (TEnum)(object)value;
                        if (values.Contains(enumValue))
                        {
                            throw new InvalidOperationException($"Duplicate value: {enumValue}");
                        }
                        values.Add(enumValue);
                    }
                    else
                    {
                        throw new JsonException($"Invalid value: {value}");
                    }
                }
                else
                {
                    throw new JsonException($"Expected integer value for {typeof(TEnum).Name}.");
                }
            }
            else
            {
                throw new JsonException($"Unexpected token type: {reader.TokenType}");
            }
        }

        throw new JsonException("Unexpected end of array.");
    }

    public override void Write(Utf8JsonWriter writer, TCollection collection, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (var value in collection.SimplifiedValues)
        {
            writer.WriteNumberValue(Convert.ToInt32(value));
        }
        writer.WriteEndArray();
    }
}