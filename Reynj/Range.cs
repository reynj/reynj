using System;

namespace Reynj
{
    public class Range<T> : IEquatable<Range<T>> //, IComparable<Range<T>>
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
            if (ReferenceEquals(other, null))
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

        public static bool operator ==(Range<T> leftRange, Range<T> rightRange)
        {
            // Check for null.
            if (ReferenceEquals(leftRange, null))
            {
                if (ReferenceEquals(rightRange, null))
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

        public override string ToString()
        {
            return $"Range({_start}, {_end})";
        }
    }
}
