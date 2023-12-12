#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif
using Reynj.Extensions;

namespace Reynj.Linq
{
    /// <summary>
    /// Provides a set of static (Shared in Visual Basic) methods for querying objects that implement <see cref="IEnumerable{TRange}" />
    /// </summary>
    public static partial class Enumerable
    {
        /// <summary>
        /// Returns the inversed ranges from a sequence 
        /// </summary>
        /// <param name="source">An <see cref="IEnumerable{TRange}"></see> to inverse.</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>An <see cref="IEnumerable{TRange}"></see> that contains the inversed elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        /// <exception cref="InvalidOperationException">When the <typeparamref name="T"/> has no MinValue and/or MaxValue defined.</exception>
#if NET7_0_OR_GREATER
        public static IEnumerable<Range<T>> Inverse<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] T>(this IEnumerable<Range<T>> source)
#else
        public static IEnumerable<Range<T>> Inverse<T>(this IEnumerable<Range<T>> source)
#endif

            where T : IComparable
        {
            var minValue = typeof(T).MinValue<T>();
            var maxValue = typeof(T).MaxValue<T>();

            return PrivateInverse(source, minValue, maxValue);
        }

        /// <summary>
        /// Returns the inversed ranges from a sequence 
        /// </summary>
        /// <param name="source">An <see cref="IEnumerable{TRange}"></see> to inverse.</param>
        /// <param name="minValue">MinValue of the given type</param>
        /// <param name="maxValue">MaxValue of the given type</param>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <returns>An <see cref="IEnumerable{TRange}"></see> that contains the inversed elements from the source sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source">source</paramref> is null.</exception>
        /// <exception cref="ArgumentException">If <paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
        public static IEnumerable<Range<T>> Inverse<T>(this IEnumerable<Range<T>> source, T minValue, T maxValue)
            where T : IComparable
        {
            return PrivateInverse(source, minValue, maxValue);
        }

        private static IEnumerable<Range<T>> PrivateInverse<T>(IEnumerable<Range<T>> source, T minValue, T maxValue)
            where T : IComparable
        {
#if NET6_0_OR_GREATER && !NETSTANDARD
            ArgumentNullException.ThrowIfNull(source);
#else
        if (source == null)
            throw new ArgumentNullException(nameof(source));
#endif

            if (minValue.CompareTo(maxValue) > 0)
                throw new ArgumentException($"{nameof(maxValue)} must be greater than or equal to {nameof(minValue)}.",
                    nameof(maxValue));

            var reducedSource = source
                .Reduce() // Sorts the collection, merges overlapping and touching ranges and removes empty ranges
                .ToList();

            var inversed = new List<Range<T>>();

            // If the ranges are empty, return a single range between minvalue & maxvalue
            if (reducedSource.Count == 0)
                return new[] { new Range<T>(minValue, maxValue) };

            // First range is from MinValue to the start of the first range
            if (reducedSource.First().Start.CompareTo(minValue) > 0)
            {
                inversed.Add(new Range<T>(minValue, reducedSource.First().Start));
            }

            // Add a range for each Gap between the currentRange and the next Range
            for (var i = 0; i < reducedSource.Count - 1; i++)
            {
                var range = reducedSource[i];
                var nextRange = reducedSource[i+1];

                inversed.Add(range.Gap(nextRange));
            }

            // Last range is from the end of the last range to MaxValue
            if (reducedSource.Last().End.CompareTo(maxValue) < 0)
            {
                inversed.Add(new Range<T>(reducedSource.Last().End, maxValue));
            }

            return inversed;
        }
    }
}