using System;

namespace Reynj
{
    /// <summary>
    /// An immutable class that is a representation of a Range.
    /// Where a Range is a range of values determined by a start and an end.
    /// </summary>
    /// <typeparam name="T">Every type that implements <see cref="IComparable"/></typeparam>
    public class Range<T> : IEquatable<Range<T>>, IComparable<Range<T>>, IComparable
        where T : IComparable
    {
        private readonly T _start;
        private readonly T _end;

        /// <summary>
        /// Creates a new <see cref="Range{T}"/>
        /// </summary>
        /// <param name="start">Start of the Range</param>
        /// <param name="end">End of the Range</param>
        /// <exception cref="ArgumentException">If <paramref name="start"/> is greater than <paramref name="end"/>.</exception>
        public Range(T start, T end)
        {
            if (start.CompareTo(end) > 0)
                throw new ArgumentException($"{nameof(end)} must be greater than or equal to {nameof(start)}",
                    nameof(end));

            _start = start;
            _end = end;
        }

        /// <summary>
        /// Start of the <see cref="Range{T}"/>
        /// </summary>
        public T Start => _start;

        /// <summary>
        /// End of the <see cref="Range{T}"/>
        /// </summary>
        /// <remarks>The End is not part of the range, it marks the ending via &lt; End.</remarks>
        public T End => _end;

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is part of the <see cref="Range{T}"/>
        /// </summary>
        /// <param name="value">Value the check against the <see cref="Range{T}"/></param>
        /// <returns>true if the specified <paramref name="value"/> is within the <see cref="Range{T}"/>; otherwise, false.</returns>
        /// <remarks>When specified <paramref name="value"/> is null, false is returned.</remarks>
        public bool Includes(T value)
        {
            if (value == null)
                return false;

            return _start.CompareTo(value) > 0 && _end.CompareTo(value) < 0;
        }

        /// <summary>
        /// Returns true if the Range starts and ends with the same value
        /// </summary>
        /// <returns>true if the specified <see cref="Start"/> is equal to the <see cref="End"/>; otherwise, false.</returns>
        public bool IsEmpty()
        {
            return _start.Equals(_end);
        }

        /// <summary>
        /// Determines whether the specified <see cref="Range{T}"/> is equal to the current <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="other">The <see cref="Range{T}"/> to compare with the current object.</param>
        /// <returns>true if the specified <see cref="Range{T}"/> is equal to the current <see cref="Range{T}"/>; otherwise, false.</returns>
        public bool Equals(Range<T> other)
        {
            // If parameter is null, return false.
            if (other is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (GetType() != other.GetType())
            {
                return false;
            }

            return _start.Equals(other._start) && _end.Equals(other._end);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as Range<T>);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (_start.GetHashCode() << 2) ^ _end.GetHashCode();
        }

        /// <inheritdoc />
        public int CompareTo(Range<T> other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other is null) return 1;

            // First compare the Start, if they are the same compare the End
            var result = _start.CompareTo(other._start);
            if (result == 0)
                result = _end.CompareTo(other._end);

            return result;
        }

        /// <inheritdoc />
        public int CompareTo(object obj)
        {
            return CompareTo(obj as Range<T>);
        }

        /// <summary>
        /// Determines whether two specified Ranges have the same value.
        /// </summary>
        /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
        /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
        /// <returns><c>true</c> if the <paramref name="leftRange"/> and <paramref name="rightRange"/> parameters have the same value; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Range<T> leftRange, Range<T> rightRange)
        {
            // Check for null.
            if (leftRange is null)
            {
                if (rightRange is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }

            // Equals handles the case of null on right side.
            return leftRange.Equals(rightRange);
        }

        /// <summary>
        /// Determines whether two specified Ranges have different values.
        /// </summary>
        /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
        /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
        /// <returns>true if the value of <paramref name="leftRange" /> is different from the value of <paramref name="rightRange" />; otherwise, false.</returns>
        public static bool operator !=(Range<T> leftRange, Range<T> rightRange)
        {
            return !(leftRange == rightRange); 
        }

        /// <summary>
        /// Determines whether one specified <see cref="Range{T}"/> is greater than another specified <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
        /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
        /// <returns>true if <paramref name="leftRange" /> is greater than <paramref name="rightRange" />; otherwise, false.</returns>
        public static bool operator >(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) == 1;
        }

        /// <summary>
        /// Determines whether one specified <see cref="Range{T}"/> is lower than another specified <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
        /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
        /// <returns>true if <paramref name="leftRange" /> is lower than <paramref name="rightRange" />; otherwise, false.</returns>
        public static bool operator <(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) == -1;
        }

        /// <summary>
        /// Determines whether one specified <see cref="Range{T}"/> represents a range that is the same as or lower than another specified <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
        /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
        /// <returns>true if <paramref name="leftRange" /> is the same as or lower than <paramref name="rightRange" />; otherwise, false.</returns>
        public static bool operator >=(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) >= 0;
        }

        /// <summary>
        /// Determines whether one specified <see cref="Range{T}"/> represents a range that is the same as or greater than another specified <see cref="Range{T}"/>.
        /// </summary>
        /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
        /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
        /// <returns>true if <paramref name="leftRange" /> is the same as or greater than <paramref name="rightRange" />; otherwise, false.</returns>
        public static bool operator <=(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) <= 0;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"Range({_start}, {_end})";
        }
    }
}