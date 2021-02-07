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
        /// Returns the highest End of all Ranges in the collection
        /// </summary>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"></see> to look for the highest value.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>The highest End of all Ranges</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        /// <exception cref="NotSupportedException">When there are no items in the <see cref="T:System.Collections.Generic.IEnumerable`1"></see></exception>
        public static T Highest<T>(this IEnumerable<Range<T>> source)
            where T : IComparable
        {
            if (source == null) 
                throw new ArgumentNullException(nameof(source));

            var highestRange = source
                .OrderByDescending(r => r)
                .FirstOrDefault();

            return highestRange != null ? highestRange.End : throw new NotSupportedException("Highest is not supported on an empty collection.");
        }
    }
}