using System;
using System.Collections.Generic;
using System.Linq;

namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="System.Collections.Generic.IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Unions two sequences, meanwhile also reducing the result
        /// </summary>
        /// <param name="first">The first sequence to union.</param>
        /// <param name="second">The sequence to union to the first sequence.</param>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"></see> that contains the unionized elements of the two input sequences.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="first">first</paramref> or <paramref name="second">second</paramref> is null.</exception>
        public static IEnumerable<Range<T>> Union<T>(this IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
        {
            if (first == null) 
                throw new ArgumentNullException(nameof(first));
            if (second == null) 
                throw new ArgumentNullException(nameof(second));

            return first
                .Concat(second) // Merge the two range collections
                .Reduce(); // Merge overlapping and touching ranges
        }
    }
}