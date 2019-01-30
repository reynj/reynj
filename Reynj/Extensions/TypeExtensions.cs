using System;

using System.Runtime.CompilerServices;
[assembly:InternalsVisibleTo("Reynj.UnitTests")]

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

            if (field.IsLiteral && !field.IsInitOnly)
                return (T) field.GetRawConstantValue();

            return (T) field.GetValue(null);
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

            if (field.IsLiteral && !field.IsInitOnly)
                return (T) field.GetRawConstantValue();

            return (T) field.GetValue(null);
        }
    }
}