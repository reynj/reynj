namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Unions two sequences, meanwhile also reducing the result (AND)
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Union_(set_theory)"/>
        /// <param name="first">The first sequence to union.</param>
        /// <param name="second">The sequence to union to the first sequence.</param>
        /// <typeparam name="T">The type of the elements of the input sequences.</typeparam>
        /// <returns>An <see cref="IEnumerable{TRange}"></see> that contains the unionized elements of the two input sequences.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="first">first</paramref> or <paramref name="second">second</paramref> is null.</exception>
        public static IEnumerable<Range<T>> Union<T>(this IEnumerable<Range<T>> first, IEnumerable<Range<T>> second)
            where T : IComparable
        {
            if (first == null) 
                throw new ArgumentNullException(nameof(first));
            if (second == null) 
                throw new ArgumentNullException(nameof(second));

            return first
                .Concat(second) // Merge the two range collections (Concat is used instead of Union, because the Reduce removes the doubles)
                .Reduce(); // Merge overlapping and touching ranges
        }
    }
}