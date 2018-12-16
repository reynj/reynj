using System;

namespace Reynj
{
    public class Range<T> where T : IComparable
    {
        private readonly T _start;
        private readonly T _end;

        public Range(T start, T end)
        {
            if (start.CompareTo(end) > 0)
                throw new ArgumentException($"{nameof(end)} must be greater than or equal to {nameof(start)}", nameof(end));

            _start = start;
            _end = end;
        }

        public T Start => _start;
        public T End => _end;
    }
}
