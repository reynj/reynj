namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Check if a collection of Ranges only contains touching Ranges and form a contiguous sequence.
        /// </summary>
        /// <param name="source">An <see cref="IEnumerable{TRange}"></see> to check for contiguousness.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>Returns true if the collection of Ranges only contains touching Ranges and form a contiguous sequence, otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        public static bool IsContiguous<T>(this IEnumerable<Range<T>> source)
            where T : IComparable
        {
#if NET6_0_OR_GREATER && !NETSTANDARD
            ArgumentNullException.ThrowIfNull(source);
#else
        if (source == null)
            throw new ArgumentNullException(nameof(source));
#endif

            //if (source.IsSingle())
            //    return true;

#if NETSTANDARD2_0
            return source
                .OrderBy(r => r) // It is easier to aggregate if the Ranges are ordered
                .Aggregate<Range<T>, Range<T>, bool>(
                    null!,
                    (previous, current) =>
                    {
                        if (previous == null || (!previous.IsEmpty() && previous.Touches(current)))
                            return current;

                        return Range<T>.Empty;
                    },
                    result => result != null && !result.IsEmpty()
                );
#else
            return source
                .OrderBy(r => r) // It is easier to aggregate if the Ranges are ordered
                .Aggregate<Range<T>?, Range<T>?, bool>(
                    null, 
                    (previous, current) =>
                    {
                        if (previous == null || (!previous.IsEmpty() && previous.Touches(current)))
                            return current;

                        return Range<T>.Empty;
                    },
                    result => result != null && !result.IsEmpty()
                );
#endif
        }
    }
}