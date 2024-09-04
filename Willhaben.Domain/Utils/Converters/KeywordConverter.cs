using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters
{
    public class KeywordJsonConverter<T> : JsonConverter<T> where T : Keyword
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string keywordValue = reader.GetString();
            return (T)Activator.CreateInstance(typeof(T), keywordValue);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Value);
        }
    }

    public class KeywordListJsonConverter<T> : JsonConverter<List<T>> where T : Keyword
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected an array.");
            }

            var list = new List<T>();
            var converter = new KeywordJsonConverter<T>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return list;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    var keyword = converter.Read(ref reader, typeof(T), options);
                    list.Add(keyword);
                }
                else
                {
                    throw new JsonException($"Unexpected token type: {reader.TokenType}");
                }
            }

            throw new JsonException("Unexpected end of JSON.");
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            foreach (var item in value)
            {
                new KeywordJsonConverter<T>().Write(writer, item, options);
            }

            writer.WriteEndArray();
        }
    }
}