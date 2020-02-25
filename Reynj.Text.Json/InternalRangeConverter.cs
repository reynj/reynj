using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reynj.Text.Json
{
    /// <inheritdoc />
    internal class RangeConverter<T> : JsonConverter<Range<T>>
        where T : IComparable
    {
        private const string StartName = "Start";
        private const string EndName = "End";

        // "encoder: null" is used since the literal values of "Start" and "End" should not normally be escaped
        // unless a custom encoder is used that escapes these ASCII characters (rare).
        // Also by not specifying an encoder allows the values to be cached statically here.
        private readonly JsonEncodedText _startName = JsonEncodedText.Encode(StartName);
        private readonly JsonEncodedText _endName = JsonEncodedText.Encode(EndName);

        /// <inheritdoc />
        public override Range<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            T start = default!;
            var startSet = false;

            T end = default!;
            var endSet = false;

            // Get the first property.
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var propertyName = reader.GetString()!;
            if (propertyName == StartName)
            {
                start = ReadProperty<T>(ref reader, typeToConvert, options);
                startSet = true;
            }
            else if (propertyName == EndName)
            {
                end = ReadProperty<T>(ref reader, typeToConvert, options);
                endSet = true;
            }
            else
            {
                throw new JsonException();
            }

            // Get the second property.
            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            propertyName = reader.GetString()!;
            if (propertyName == EndName)
            {
                end = ReadProperty<T>(ref reader, typeToConvert, options);
                endSet = true;
            }
            else if (propertyName == StartName)
            {
                start = ReadProperty<T>(ref reader, typeToConvert, options);
                startSet = true;
            }
            else
            {
                throw new JsonException();
            }

            if (!startSet || !endSet)
            {
                throw new JsonException();
            }

            reader.Read();

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            return new Range<T>(start, end);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Range<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            WriteProperty(writer, value.Start, _startName, options);
            WriteProperty(writer, value.End, _endName, options);
            writer.WriteEndObject();
        }

        private static TValue ReadProperty<TValue>(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            where TValue : IComparable
        {
            TValue value;

            // Attempt to use existing converter first before re-entering through JsonSerializer.Deserialize().
            // The default converter for objects does not parse null objects as null, so it is not used here.
            if (typeToConvert != typeof(object) && options?.GetConverter(typeToConvert) is JsonConverter<TValue> keyConverter)
            {
                reader.Read();
                value = keyConverter.Read(ref reader, typeToConvert, options);
            }
            else
            {
                value = JsonSerializer.Deserialize<TValue>(ref reader, options);
            }

            return value;
        }

        private static void WriteProperty<TValue>(Utf8JsonWriter writer, TValue value, JsonEncodedText name, JsonSerializerOptions options)
            where TValue : IComparable
        {
            var typeToConvert = typeof(TValue);

            writer.WritePropertyName(name);

            // Attempt to use existing converter first before re-entering through JsonSerializer.Serialize().
            // The default converter for object does not support writing.
            if (typeToConvert != typeof(object) && options?.GetConverter(typeToConvert) is JsonConverter<TValue> keyConverter)
            {
                keyConverter.Write(writer, value, options);
            }
            else
            {
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}