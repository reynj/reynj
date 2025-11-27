namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns the lowest Start of all Ranges in the collection
        /// </summary>
        /// <param name="source">An <see cref="IEnumerable{TRange}"></see> to look for the lowest value.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>The lowest Start of all Ranges</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        /// <exception cref="NotSupportedException">When there are no items in the <see cref="IEnumerable{TRange}"></see></exception>
        public static T Lowest<T>(this IEnumerable<Range<T>> source)
            where T : IComparable
        {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(source);

            var lowestRange = source.MinBy(r => r);
#else
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        var lowestRange = source
            .OrderBy(r => r)
            .FirstOrDefault();
#endif

            return lowestRange != null ? lowestRange.Start : throw new NotSupportedException("Lowest is not supported on an empty collection.");
        }
    }
}