using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Reynj.Newtonsoft.Json")]

namespace Reynj.Extensions
{
    /// <summary>
    /// Extension methods that work on any type
    /// </summary>
    internal static class TypeExtensions
    {
        /// <summary>
        /// Represents the smallest possible value of the given type
        /// </summary>
        /// <typeparam name="T">Type to get the MinValue of</typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T MinValue<T>(this Type self)
        {
            var field = self.GetField(nameof(MinValue));

            if (field == null)
                throw new InvalidOperationException($"The type {self.Name} does not contain a field with name {nameof(MinValue)}.");

            if (field.IsLiteral && !field.IsInitOnly)
                return (T) (field.GetRawConstantValue() ?? throw new InvalidOperationException($"The value of the field {nameof(MinValue)} on {self.Name} is null and not allowed."));

            return (T) (field.GetValue(null) ?? throw new InvalidOperationException($"The value of the field {nameof(MinValue)} on {self.Name} is null and not allowed."));
        }

        /// <summary>
        /// Represents the largest possible value of the given type
        /// </summary>
        /// <typeparam name="T">Type to get the MaxValue of</typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static T MaxValue<T>(this Type self)
        {
            var field = self.GetField(nameof(MaxValue));

            if (field == null)
                throw new InvalidOperationException($"The type {self.Name} does not contain a field with name {nameof(MaxValue)}.");

            if (field.IsLiteral && !field.IsInitOnly)
                return (T)(field.GetRawConstantValue() ?? throw new InvalidOperationException($"The value of the field {nameof(MaxValue)} on {self.Name} is null and not allowed."));

            return (T)(field.GetValue(null) ?? throw new InvalidOperationException($"The value of the field {nameof(MaxValue)} on {self.Name} is null and not allowed."));
        }

        /// <summary>
        /// Returns a collection of types that are base types of the given type, including the type itself
        /// </summary>
        /// <param name="type">Type to get all base types of</param>
        /// <returns>A collection of types</returns>
        public static IEnumerable<Type> BaseTypesAndSelf(this Type? type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
        }
    }
}