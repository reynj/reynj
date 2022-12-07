using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Reynj.Extensions;

namespace Reynj.Newtonsoft.Json
{
    /// <inheritdoc />
    public class RangeConverter : JsonConverter
    {
        private const string StartName = "Start";
        private const string EndName = "End";

        private static Type? GetValueType(Type objectType)
        {
            return objectType
                .BaseTypesAndSelf()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Range<>))
                .Select(t => t.GetGenericArguments()[0])
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return GetValueType(objectType) != null;
        }

        /// <inheritdoc />
        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            object? start = null;
            var startSet = false;

            object? end = null;
            var endSet = false;

            var valueType = GetValueType(objectType);

            reader.Read();
            
            while (reader.TokenType == JsonToken.PropertyName)
            {
                var propertyName = reader.Value!.ToString();
                if (string.Equals(propertyName, StartName, StringComparison.OrdinalIgnoreCase))
                {
                    reader.Read();

                    start = serializer.Deserialize(reader, valueType);
                    startSet = true;
                }
                else if (string.Equals(propertyName, EndName, StringComparison.OrdinalIgnoreCase))
                {
                    reader.Read();

                    end = serializer.Deserialize(reader, valueType);
                    endSet = true;
                }
                else
                {
                    reader.Skip();
                }

                reader.Read();
            }

            if (reader.TokenType != JsonToken.EndObject)
            {
                throw new JsonException();
            }

            if (!startSet || !endSet)
            {
                // ReSharper disable once PossibleNullReferenceException
                return objectType.GetField(nameof(Range<int>.Empty), BindingFlags.Public | BindingFlags.Static)?.GetValue(objectType);
            }

            return Activator.CreateInstance(objectType, start, end);
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var rangeType = value.GetType();
            var valueType = GetValueType(rangeType);

            var resolver = serializer.ContractResolver as DefaultContractResolver;

            // ReSharper disable once PossibleNullReferenceException
            var isEmpty = (bool) (rangeType.GetMethod(nameof(Range<int>.IsEmpty))?.Invoke(value, Array.Empty<object>()) ?? true);

            writer.WriteStartObject();

            if (!isEmpty)
            {
                writer.WritePropertyName(resolver != null ? resolver.GetResolvedPropertyName(StartName) : StartName);
                serializer.Serialize(writer, rangeType.GetProperty(nameof(Range<int>.Start))?.GetValue(value), valueType);

                writer.WritePropertyName(resolver != null ? resolver.GetResolvedPropertyName(EndName) : EndName);
                serializer.Serialize(writer, rangeType.GetProperty(nameof(Range<int>.End))?.GetValue(value), valueType);
            }
                
            writer.WriteEndObject();
        }
    }
}