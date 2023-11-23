using System.Collections;

namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns true when the <see cref="IEnumerable"></see> contains exactly one element.
        /// </summary>
        /// <param name="source">An <see cref="IEnumerable"></see> to look for the lowest value.</param>
        /// <returns>True when the <see cref="IEnumerable"></see> contains exactly one element, otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        public static bool IsSingle(this IEnumerable source)
        {
#if NET6_0_OR_GREATER && !NETSTANDARD
            ArgumentNullException.ThrowIfNull(source);
#else
        if (source == null)
            throw new ArgumentNullException(nameof(source));
#endif

            var enumerator = source.GetEnumerator();

            return enumerator.MoveNext() && !enumerator.MoveNext();
        }
    }
}
