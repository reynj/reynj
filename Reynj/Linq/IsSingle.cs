using System;
using System.Collections;

namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="System.Collections.Generic.IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns true when the <see cref="T:System.Collections.Generic.IEnumerable"></see> contains exactly one element.
        /// </summary>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable"></see> to look for the lowest value.</param>
        /// <returns>True when the <see cref="T:System.Collections.Generic.IEnumerable"></see> contains exactly one element, otherwise false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        public static bool IsSingle(this IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var enumerator = source.GetEnumerator();

            return enumerator.MoveNext() && !enumerator.MoveNext();
        }
    }
}
