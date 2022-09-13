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
        /// Check if a collection of Ranges only contains touching Ranges and form a contiguous sequence.
        /// </summary>
        /// <param name="first">The first sequence.</param>
        /// <param name="second">The sequence to do the overlap comparison with.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>Returns true if the collection of Ranges have ranges that <see cref="Range{T}.Overlaps"/>, otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first">first</paramref> or <paramref name="second">second</paramref> is null.</exception>
        public static bool Overlaps<T>(this IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
        {
            if (first == null) 
                throw new ArgumentNullException(nameof(first));
            if (second == null) 
                throw new ArgumentNullException(nameof(second));

            // By reducing both sequences the overlapping logic has less Ranges to check
            var firstReduced = first.Reduce().ToList();
            var secondReduced = second.Reduce().ToList();

            // If one of the lists is empty, the overlap is always false
            if (!firstReduced.Any() || !secondReduced.Any())
            {
                return false; 
            }

            // Check if both sequences have an overlap as a whole, if not then the individual Ranges cannot overlap
            if (firstReduced.Highest().CompareTo(secondReduced.Lowest()) < 0 || secondReduced.Highest().CompareTo(firstReduced.Lowest()) < 0)
            {
                 return false; 
            }

            // TODO: Make sure we only loop each IEnumerable once by skipping until they are in the same range value
            // Loop over the first list and find the overlapping ranges with the second list, then return the intersection
            return firstReduced
                .SelectMany(firstRange => secondReduced
                    .SkipWhile(secondRange =>  firstRange.IsCompletelyBefore(secondRange) || secondRange.IsCompletelyBefore(firstRange))
                    .TakeWhile(secondRange => !(firstRange.IsCompletelyBehind(secondRange) || secondRange.IsCompletelyBehind(firstRange)))
                    .Any(firstRange.Overlaps));
        }
    }
}