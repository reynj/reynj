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
        /// Keeps the elements which are in either of the sequences and not in their intersection. (XOR)
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Symmetric_difference"/>
        /// <see href="https://en.wikipedia.org/wiki/Exclusive_or"/>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The sequence to do the exclusive comparison with.</param>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"></see> that contains the exclusive elements of the two input sequences.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="first">first</paramref> or <paramref name="second">second</paramref> is null.</exception>
        public static IEnumerable<Range<T>> Difference<T>(this IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
        {
            if (first == null) 
                throw new ArgumentNullException(nameof(first));
            if (second == null) 
                throw new ArgumentNullException(nameof(second));

            // By reducing both sequences the set difference logic becomes easier
            var firstReduced = first.Reduce().ToList();
            var secondReduced = second.Reduce().ToList();


        }
    }
}