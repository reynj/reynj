namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns reduced elements from a sequence by removing the empty ones and merging the overlapping and touching ones
        /// </summary>
        /// <param name="source">An <see cref="IEnumerable{TRange}"></see> to reduce.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>An <see cref="IEnumerable{TRange}"></see> that contains the reduced elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        public static IEnumerable<Range<T>> Reduce<T>(this IEnumerable<Range<T>> source) // Reduce (In functional programming, fold (also termed reduce, accumulate, aggregate, compress, or inject))
            where T : IComparable
        {
            if (source == null) 
                throw new ArgumentNullException(nameof(source));

            return source
                .Where(r => !r.IsEmpty()) // Remove all empty Ranges
                .OrderBy(r => r) // It is easier to aggregate if the Ranges are ordered
                .Aggregate<Range<T>, ICollection<Range<T>>>(
                    new List<Range<T>>(),
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