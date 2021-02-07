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
#if NETSTANDARD2_0
        public override Range<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
#else
        public override Range<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
#endif
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            var valueType = typeof(T);

            T start = default;
            var startSet = false;

            T end = default;
            var endSet = false;

            reader.Read();

            while (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString()!;
                if (string.Equals(propertyName, StartName, StringComparison.OrdinalIgnoreCase))
                {
                    start = ReadProperty<T>(ref reader, valueType, options);
                    startSet = true;
                }
                else if (string.Equals(propertyName, EndName, StringComparison.OrdinalIgnoreCase))
                {
                    end = ReadProperty<T>(ref reader, valueType, options);
                    endSet = true;
                }
                else
                {
                    reader.Skip();
                }

                reader.Read();
            }

            if (reader.TokenType != JsonTokenType.EndObject)
            {
                throw new JsonException();
            }

            if (!startSet || !endSet)
            {
                return Range<T>.Empty;
            }

            return new Range<T>(start!, end!);
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, Range<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            if (!value.IsEmpty())
            {
                WriteProperty(writer, value.Start, _startName, options);
                WriteProperty(writer, value.End, _endName, options);
            }

            writer.WriteEndObject();
        }

        private static TValue? ReadProperty<TValue>(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions? options)
        {
            // Attempt to use existing converter first before re-entering through JsonSerializer.Deserialize().
            // The default converter for objects does not parse null objects as null, so it is not used here.
            if (typeToConvert != typeof(object) && options?.GetConverter(typeToConvert) is JsonConverter<TValue> valueConverter)
            {
                reader.Read();
                return valueConverter.Read(ref reader, typeToConvert, options);
            }

            return JsonSerializer.Deserialize<TValue>(ref reader, options);
        }

        private static void WriteProperty<TValue>(Utf8JsonWriter writer, TValue value, JsonEncodedText name, JsonSerializerOptions? options)
        {
            var typeToConvert = typeof(TValue);

            writer.WritePropertyName(name);

            // Attempt to use existing converter first before re-entering through JsonSerializer.Serialize().
            // The default converter for object does not support writing.
            if (typeToConvert != typeof(object) && options?.GetConverter(typeToConvert) is JsonConverter<TValue> valueConverter)
            {
                valueConverter.Write(writer, value, options);
            }
            else
            {
                JsonSerializer.Serialize(writer, value, options);
            }
        }
    }
}