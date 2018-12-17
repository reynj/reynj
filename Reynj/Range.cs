using System;

namespace Reynj
{
    public class Range<T> : IEquatable<Range<T>>, IComparable<Range<T>>, IComparable
        where T : IComparable
    {
        private readonly T _start;
        private readonly T _end;

        public Range(T start, T end)
        {
            if (start.CompareTo(end) > 0)
                throw new ArgumentException($"{nameof(end)} must be greater than or equal to {nameof(start)}",
                    nameof(end));

            _start = start;
            _end = end;
        }

        public T Start => _start;
        public T End => _end;

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

        public override bool Equals(object obj)
        {
            return Equals(obj as Range<T>);
        }

        public override int GetHashCode()
        {
            return (_start.GetHashCode() << 2) ^ _end.GetHashCode();
        }

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

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Range<T>);
        }

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

        public static bool operator !=(Range<T> leftRange, Range<T> rightRange)
        {
            return !(leftRange == rightRange);
        }

        public static bool operator >(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) == 1;
        }

        public static bool operator <(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) == -1;
        }

        public static bool operator >=(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) >= 0;
        }

        public static bool operator <=(Range<T> leftRange, Range<T> rightRange)
        {
            return leftRange.CompareTo(rightRange) <= 0;
        }

        public override string ToString()
        {
            return $"Range({_start}, {_end})";
        }
    }
}