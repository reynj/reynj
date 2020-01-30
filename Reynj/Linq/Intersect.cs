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
        /// Keeps the intersection of two sequences
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Intersection_(set_theory)"/>
        /// <param name="first">The first sequence to intersect.</param>
        /// <param name="second">The sequence to union to the first intersect.</param>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"></see> that contains the intersected elements of the two input sequences.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="first">first</paramref> or <paramref name="second">second</paramref> is null.</exception>
        public static IEnumerable<Range<T>> Intersect<T>(this IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
        {
            if (first == null) 
                throw new ArgumentNullException(nameof(first));
            if (second == null) 
                throw new ArgumentNullException(nameof(second));

            // By reducing both sequences the intersection logic becomes easier
            var firstReduced = first.Reduce().ToList();
            var secondReduced = second.Reduce().ToList();

            // If one of the lists is empty, the intersection is always empty
            if (!firstReduced.Any() || !secondReduced.Any())
            {
                return new Range<T>[] {}; 
            }

            // Check if both sequences overlap, if not the intersection is empty
            if (firstReduced.Highest().CompareTo(secondReduced.Lowest()) < 0 || secondReduced.Highest().CompareTo(firstReduced.Lowest()) < 0)
            {
                 return new Range<T>[] {}; 
            }

            // Loop over the first list and find the overlapping ranges with the second list, then return the intersection
            return firstReduced
                .SelectMany(firstRange => secondReduced
                    .SkipWhile(secondRange =>  firstRange.IsCompletelyBefore(secondRange) || secondRange.IsCompletelyBefore(firstRange))
                    .TakeWhile(secondRange => !(firstRange.IsCompletelyBehind(secondRange) || secondRange.IsCompletelyBehind(firstRange)))
                    .Where(firstRange.Overlaps)
                    .Select(firstRange.Intersection));
        }
    }
}