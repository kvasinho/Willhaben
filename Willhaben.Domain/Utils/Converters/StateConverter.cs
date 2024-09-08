using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters;
public class StateCollectionConverter : JsonConverter<StateCollection>
    {
        public override StateCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Expected start of array.");
            }

            var states = new List<State>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    var collection = new StateCollection();
                    collection.SetSimplifiedValues(states);
                    return collection;
                }

                if (reader.TokenType == JsonTokenType.Number)
                {
                    if (reader.TryGetInt32(out int stateValue))
                    {
                        if (Enum.IsDefined(typeof(State), stateValue))
                        {
                            var state = (State)stateValue;
                            if (states.Contains(state))
                            {
                                throw new EnumKeyExistsException<State>(state);
                            }
                            states.Add(state);
                        }
                        else
                        {
                            throw new StringEnumConversionException($"Invalid State value: {stateValue}");
                        }
                    }
                    else
                    {
                        throw new JsonException($"Expected integer value for State.");
                    }
                }
                else
                {
                    throw new JsonException($"Unexpected token type: {reader.TokenType}");
                }
            }

            throw new JsonException("Unexpected end of array.");
        }

        public override void Write(Utf8JsonWriter writer, StateCollection stateCollection, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var state in stateCollection.SimplifiedValues)
            {
                writer.WriteNumberValue((int)state);
            }
            writer.WriteEndArray();
        }
    }