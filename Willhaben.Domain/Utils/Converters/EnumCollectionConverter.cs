using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Willhaben.Domain.Models;

namespace Willhaben.Domain.Utils.Converters
{
    public class SimplifyableEnumCollectionConverter<Tenum, Tcollection> : JsonConverter<Tcollection>
        where Tenum : Enum
        where Tcollection : SimplifyableEnumCollection<Tenum>
    {
        public override Tcollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Create an instance of the collection type
            var collection = (Tcollection)Activator.CreateInstance(typeToConvert);

            // Read the JSON array
            var simplifiedValues = JsonSerializer.Deserialize<List<Tenum>>(ref reader, options);

            // Set the simplified values on the collection
            collection.SetSimplifiedValues(simplifiedValues);

            return collection;
        }

        public override void Write(Utf8JsonWriter writer, Tcollection value, JsonSerializerOptions options)
        {
            // Serialize the simplified values
            JsonSerializer.Serialize(writer, value.SimplifiedValues, options);
        }
    }
}