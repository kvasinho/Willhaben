using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters;
/*
public class LocationArrayConverter : JsonConverter<Location[]>
{
    public override ScraperConfig.Location[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException();
        }

        var locations = new List<ScraperConfig.Location>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
            {
                return locations.ToArray();
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string locationName = reader.GetString();
                if (Enum.TryParse<ScraperConfig.Location>(locationName, true, out var location))
                {
                    locations.Add(location);
                }
                else
                {
                    Console.WriteLine($"Warning: Unknown location '{locationName}' ignored.");
                }
            }
        }

        throw new JsonException();
    }
    */