using System.Collections.Generic;

namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="System.Collections.Generic.IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        // TODO: Make public after adding unit tests
        internal static bool IsSingle<T>(this IEnumerable<T> enumerable)
        {
            // Alternative
            // return enumerable.Take(2).Count() == 1;

            using var enumerator = enumerable.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return false;
            }

            return !enumerator.MoveNext();
        }
    }
}
