using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Utils.Converters;

namespace Willhaben.Domain.Models;

public interface IJsonConfig
{
    public Settings Settings { get; set; }
}

public class WillhabenJsonConfig : IJsonConfig
{
    [JsonPropertyName("settings")]
    public Settings Settings { get; set; }
    
    [JsonPropertyName("keywords")]
    public Keywords Keywords { get; set; }
}

public class EbayJsonConfig : IJsonConfig
{
    [JsonPropertyName("settings")]
    public Settings Settings { get; set; }
}

public class Settings
{
    public string Type { get; set; }
    [JsonConverter(typeof(IntervalJsonConverter))]
    public Interval Interval { get; set; } = Interval.M5;

    //[JsonConverter(typeof(DayOfWeekJsonConverter))]
    public List<DayOfWeek> Days { get; set; } = new();
    
    public TimeOnly From { get; set; }
    public TimeOnly To { get; set; }
    
}






    public class Keywords
    {
        [JsonPropertyName("fuzzy")]
        public List<string> FuzzyKeywords { get; set; }

        [JsonPropertyName("exact")]
        public List<string> ExactKeywords { get; set; }

        [JsonPropertyName("omit")]
        public List<string> OmitKeywords { get; set; }
    }

    public interface IJsonConfigFactory
    {
        Task<IJsonConfig?> CreateAsync(string filePath);
    }
    public class JsonConfigFactory : IJsonConfigFactory
    {
        public async Task<IJsonConfig?> CreateAsync(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                PropertyNameCaseInsensitive = true
            };

            try
            {
                using var stream = File.OpenRead(filePath);
                using var jsonDoc = await JsonDocument.ParseAsync(stream);
                
                var root = jsonDoc.RootElement;
                if (root.TryGetProperty("settings", out var settingsElement) && 
                    settingsElement.TryGetProperty("type", out var typeElement))
                {
                    var type = typeElement.GetString();
                    return type?.ToLower() switch
                    {
                        "willhaben" => await JsonSerializer.DeserializeAsync<WillhabenJsonConfig>(stream, options),
                        "ebay" => await JsonSerializer.DeserializeAsync<EbayJsonConfig>(stream, options),
                        _ => throw new ArgumentException($"Unknown config type: {type}")
                    };
                }
                else
                {
                    throw new JsonException("Invalid JSON structure: missing 'settings.type' property");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the config: {ex.Message}");
                throw;
            }
        }
    }



 
    

/*
public class DayOfWeekDictionaryConverter : JsonConverter<Dictionary<DayOfWeek, DayTime>>
{
    public override Dictionary<DayOfWeek, DayTime> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = new Dictionary<DayOfWeek, DayTime>();

        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return result;

            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();

            var propertyName = reader.GetString();
            var dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), propertyName);

            reader.Read();

            var dayTime = JsonSerializer.Deserialize<DayTime>(ref reader, options);
            result.Add(dayOfWeek, dayTime);
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<DayOfWeek, DayTime> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key.ToString());
            JsonSerializer.Serialize(writer, kvp.Value, options);
        }

        writer.WriteEndObject();
    }
}


public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
    private readonly string _timeFormat = "HH:mm";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return TimeOnly.ParseExact(reader.GetString(), _timeFormat);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(_timeFormat));
    }
}
*/
/*
    public class DayOfWeekListConverter : JsonConverter<List<DayOfWeek>>
    {
        public override List<DayOfWeek> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                    return days;
                }

                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException("Expected string value for day of week.");
                }

                var dayString = reader.GetString();
                if (Enum.TryParse<DayOfWeek>(dayString, true, out var day))
                {
                    days.Add(day);
                }
                else
                {
                    throw new JsonException($"Invalid day of week: {dayString}");
                }
            }

            throw new JsonException("Unexpected end of array.");
        }

        public override void Write(Utf8JsonWriter writer, List<DayOfWeek> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var day in value)
            {
                writer.WriteStringValue(day.ToString());
            }
            writer.WriteEndArray();
        }
    }
    */