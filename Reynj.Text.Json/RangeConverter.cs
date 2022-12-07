using System.Text.Json;
using System.Text.Json.Serialization;

namespace Reynj.Text.Json
{
    /// <inheritdoc />
    public class RangeConverter : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
                return false;

            var type = typeToConvert;

            if (!type.IsGenericTypeDefinition)
                type = type.GetGenericTypeDefinition();

            return type == typeof(Range<>);
        }

        /// <inheritdoc />
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var keyType = typeToConvert.GenericTypeArguments[0];
            var converterType = typeof(RangeConverter<>).MakeGenericType(keyType);

            return (JsonConverter?) Activator.CreateInstance(converterType);
        }
    }
}