using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Reynj
{
    /// <summary>
    /// A collection of <see cref="Range{T}"/>
    /// </summary>
    /// <typeparam name="T">Every type that implements <see cref="IComparable"/></typeparam>
    public class RangeCollection<T> : Collection<Range<T>>
        where T : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RangeCollection{T}" /> class that is empty.
        /// </summary>
        public RangeCollection()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="RangeCollection{T}" /> class as a wrapper for the specified list.</summary>
        /// <param name="ranges">The list that is wrapped by the new RangeCollection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="ranges">list</paramref> is null.</exception>
        public RangeCollection(IEnumerable<Range<T>> ranges) : base(ranges.ToList())
        {
        }

        /// <summary>
        /// The lowest Start of all Ranges in the collection
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">When there are no items in the RangeCollection</exception>
        public T Lowest()
        {
            if (Count == 0)
                throw new NotSupportedException("Lowest is not supported on an empty RangeCollection.");

            return Items.OrderBy(r => r).First().Start;
        }

        /// <summary>
        /// The highest End of a all Ranges in the collection
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">When there are no items in the RangeCollection</exception>
        public T Highest()
        {
            if (Count == 0)
                throw new NotSupportedException("Highest is not supported on an empty RangeCollection.");

            return Items.OrderBy(r => r).Last().End;
        }

        /// <summary>
        /// Aggregates all <see cref="Range{T}"/> in the <see cref="RangeCollection{T}" />
        /// Removing the empty Ranges and merging all overlapping and touching Ranges
        /// </summary>
        /// <returns>A reduced <see cref="RangeCollection{T}" /></returns>
        /// <remarks>Does not change the original <see cref="RangeCollection{T}" /></remarks>
        public RangeCollection<T> Reduce() // Reduce (In functional programming, fold (also termed reduce, accumulate, aggregate, compress, or inject))
        {
            return Items
                .Where(r => !r.IsEmpty()) // Remove all empty Ranges
                .OrderBy(r => r) // It is easier to aggregate if the Ranges are ordered
                .Aggregate(
                    new RangeCollection<T>(),
                    (rc, r1) =>
                    {
                        if (rc.Any())
                        {
                            var r2 = rc.Last();

                            if (r1.Overlaps(r2) || r1.Touches(r2))
                            {
                                rc.Remove(r2);
                                rc.Add(r1.Merge(r2));
                            }
                            else
                            {
                                rc.Add(r1);
                            }
                        }
                        else
                        {
                            rc.Add(r1);
                        }

                        return rc;
                    });
        }
    }
}
