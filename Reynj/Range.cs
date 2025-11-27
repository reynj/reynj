#if NET8_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Reynj
{
    /// <summary>
    /// An immutable class that is a representation of a Range.
    /// Where a Range is a range of values determined by a start and an end.
    /// </summary>
    /// <typeparam name="T">Every type that implements <see cref="IComparable"/></typeparam>
    [Serializable]
    public class Range<T> : IEquatable<Range<T>>, IComparable<Range<T>>, IComparable, ICloneable
        where T : IComparable
    {
    private readonly T _start;
    private readonly T _end;

    /// <summary>
    ///  Represents the empty Range
    /// </summary>
    public static readonly Range<T> Empty;

    static Range()
    {
        Empty = new Range<T>();
    }

    /// <summary>Indicates whether the specified <see cref="Range{T}"/> is null or an <see cref="Range{T}.Empty"></see> <see cref="Range{T}"/>.</summary>
    /// <param name="range">The <see cref="Range{T}"/> to test</param>
    /// <returns>true if the <paramref name="range">range</paramref> parameter is null or an empty <see cref="Range{T}"/>; otherwise, false.</returns>
#if NETSTANDARD2_0
    public static bool IsNullOrEmpty(Range<T>? range)
#else
        public static bool IsNullOrEmpty([NotNullWhen(false)] Range<T>? range)
#endif
    {
        return range == null || range == Empty;
    }

    /// <summary>
    /// Creates a new <see cref="Range{T}"/> with start and end equal to their default value
    /// </summary>
    /// <remarks>Only used to create an Empty range</remarks>
    private Range()
    {
        _start = default!;
        _end = default!;
    }

    /// <summary>
    /// Creates a new <see cref="Range{T}"/> for a given start and end
    /// </summary>
    /// <param name="start">Start of the Range</param>
    /// <param name="end">End of the Range</param>
    /// <exception cref="ArgumentNullException">If <paramref name="start"/> is null.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="end"/> is null.</exception>
    /// <exception cref="ArgumentException">If <paramref name="start"/> is greater than <paramref name="end"/>.</exception>
    public Range(T start, T end)
    {
        if (start == null)
            throw new ArgumentNullException(nameof(start));

        if (end == null)
            throw new ArgumentNullException(nameof(end));

        if (start.CompareTo(end) > 0)
            throw new ArgumentException($"{nameof(end)} must be greater than or equal to {nameof(start)}.",
                nameof(end));

        _start = start;
        _end = end;
    }

        /// <summary>
        /// Creates a new <see cref="Range{T}"/> based on a <see cref="ValueTuple"/>
        /// </summary>
        /// <param name="tuple"><see cref="ValueTuple"/> representing a Range</param>
        /// <exception cref="ArgumentException">If <paramref name="tuple"/> Item1 is greater than <paramref name="tuple"/> Item2.</exception>
        public Range(ValueTuple<T, T> tuple) : this(tuple.Item1, tuple.Item2)
    {
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
    public bool Includes(T? value)
    {
        if (value == null)
            return false;

        return value.CompareTo(_start) >= 0 && value.CompareTo(_end) < 0;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Range{T}"/> is completely part of the <see cref="Range{T}"/>
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> the check against the current <see cref="Range{T}"/></param>
    /// <returns>true if the specified <see cref="Range{T}"/> is within the <see cref="Range{T}"/>; otherwise, false.</returns>
    /// <remarks>When specified <see cref="Range{T}"/> is null or Empty, false is returned.</remarks>
#if NETSTANDARD2_0
    public bool Includes(Range<T> range)
#else
        public bool Includes(Range<T>? range)
#endif
    {
        if (IsNullOrEmpty(this) || IsNullOrEmpty(range))
            return false;

        return Includes(range.Start) && (Includes(range._end) || _end.Equals(range._end));
    }

    /// <summary>
    /// Determines whether all <paramref name="values"/> are part of the <see cref="Range{T}"/>
    /// </summary>
    /// <param name="value">First value to check against the <see cref="Range{T}"/></param>
    /// <param name="values">Other values to check against the <see cref="Range{T}"/></param>
    /// <returns>true if al of the specified <paramref name="values"/> are within the <see cref="Range{T}"/>; otherwise, false.</returns>
    public bool IncludesAll(T? value, params T?[] values)
    {
        return Includes(value) && values.All(Includes);
    }

    /// <summary>
    /// Determines whether all <paramref name="values"/> are part of the <see cref="Range{T}"/>
    /// </summary>
    /// <param name="values">Values the check against the <see cref="Range{T}"/></param>
    /// <returns>true if al of the specified <paramref name="values"/> are within the <see cref="Range{T}"/>; otherwise, false.</returns>
    public bool IncludesAll(IEnumerable<T?> values)
    {
        return values.All(Includes);
    }

    /// <summary>
    /// Returns true if the Range starts and ends with the same value
    /// </summary>
    /// <returns>true if the specified <see cref="Start"/> is equal to the <see cref="End"/>; otherwise, false.</returns>
    public bool IsEmpty()
    {
        // Check for null
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (_start == null)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (_end == null)
            {
                // null == null => true
                return true;
            }

            // Only the start is null
            return false;
        }

        return _start.Equals(_end);
    }

    /// <summary>
    /// Returns true if this <see cref="Range{T}"/> overlaps with the specified <see cref="Range{T}"/>
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> to check overlapping against</param>
    /// <returns>true if the specified <see cref="Range{T}"/> is overlapping with the specified <see cref="Range{T}"/>; otherwise, false.</returns>
    /// <remarks>When specified <see cref="Range{T}"/> is null or Empty, false is returned.</remarks>
#if NETSTANDARD2_0
    public bool Overlaps(Range<T> range)
#else
        public bool Overlaps(Range<T>? range)
#endif
    {
        if (IsNullOrEmpty(this) || IsNullOrEmpty(range))
            return false;

        return range._start.CompareTo(_end) < 0 && range._end.CompareTo(_start) > 0;
    }

    /// <summary>
    /// Returns true if this <see cref="Range{T}"/> touches or abuts the specified <see cref="Range{T}"/>
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> to check touching against</param>
    /// <returns>true if the specified <see cref="Range{T}"/> touches with the specified <see cref="Range{T}"/>; otherwise, false.</returns>
    /// <remarks>When specified <see cref="Range{T}"/> is null or Empty, false is returned.</remarks>
#if NETSTANDARD2_0
    public bool Touches(Range<T> range)
#else
        public bool Touches(Range<T>? range)
#endif
    {
        if (IsNullOrEmpty(this) || IsNullOrEmpty(range))
            return false;

        return range._start.CompareTo(_end) == 0 || range._end.CompareTo(_start) == 0;
    }

    /// <summary>
    /// Returns the gap between this <see cref="Range{T}"/> and a specified <see cref="Range{T}"/>
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> to compare with</param>
    /// <returns>A <see cref="Range{T}"/> that represents the gap between the two, or <see cref="Empty"/> if there is no gap</returns>
#if NETSTANDARD2_0
    public Range<T> Gap(Range<T> range)
#else
        public Range<T> Gap(Range<T>? range)
#endif
    {
        if (IsNullOrEmpty(this) || IsNullOrEmpty(range) || Overlaps(range) || Touches(range))
            return Empty;

        Range<T> lowerRange, higherRange;
        if (CompareTo(range) < 0)
        {
            lowerRange = this;
            higherRange = range;
        }
        else
        {
            lowerRange = range;
            higherRange = this;
        }

        return new Range<T>(lowerRange._end, higherRange._start);
    }

    /// <summary>
    /// Merges two Ranges into a single <see cref="Range{T}"/>
    /// Also called Union or a Logical disjunction (OR)
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> to merge with</param>
    /// <returns>A <see cref="Range{T}"/> that represents the merged ranges</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="range"/> is null.</exception>
    /// <exception cref="ArgumentException">If <paramref name="range"/> does not overlap or touches the current <see cref="Range{T}"/></exception>
    public Range<T> Merge(Range<T> range)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(range);
#else
            if (range == null)
            throw new ArgumentNullException(nameof(range));
#endif

            if (!Overlaps(range) && !Touches(range))
            throw new ArgumentException(
                $"Merging {this} with {range} is not possible because they do not overlap nor touch each other.",
                nameof(range));

        var start = range._start.CompareTo(_start) < 0 ? range._start : _start;
        var end = range._end.CompareTo(_end) > 0 ? range._end : _end;

        return new Range<T>(start, end);
    }

    /// <summary>
    /// Split the <see cref="Range{T}"/> in two Ranges on the given <paramref name="value"/>
    /// </summary>
    /// <param name="value">Value within the Range where the split should occur</param>
    /// <returns>A ValueTuple containing both Ranges</returns>
    /// <exception cref="ArgumentException">If <paramref name="value"/> is not included in the <see cref="Range{T}"/></exception>
    public (Range<T>, Range<T>) Split(T value)
    {
        // IDEA: Return an IEnumerable<Range<T>> instead of a Tuple, and omit Range<T>.Empty

        if (!Includes(value) && !_end.Equals(value))
            throw new ArgumentException($"Splitting is not possible because the {value} is not included in {this}.",
                nameof(value));

        return (new Range<T>(_start, value), new Range<T>(value, _end));
    }

    /// <summary>
    /// Returns the intersection between two ranges
    /// Also called a Logical conjunction (AND)
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> to intersect with</param>
    /// <returns>A <see cref="Range{T}"/> that represents the intersection between the ranges</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="range"/> is null.</exception>
    /// <exception cref="ArgumentException">If <paramref name="range"/> does not overlap with the current <see cref="Range{T}"/></exception>
    public Range<T> Intersection(Range<T> range)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(range);
#else
            if (range == null)
            throw new ArgumentNullException(nameof(range));
#endif

        if (!Overlaps(range))
            throw new ArgumentException(
                $"Intersecting {this} with {range} is not possible because they do not overlap each other.",
                nameof(range));

        return new Range<T>(_start.CompareTo(range._start) > 0 ? _start : range._start,
            _end.CompareTo(range._end) < 0 ? _end : range._end);
    }

    /// <summary>
    /// Returns what is unique for both ranges
    /// Also called an Exclusive or (XOR)
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> to intersect with</param>
    /// <returns>A ValueTuple that represents holds the exclusive ranges</returns>
    /// <remarks>If both ranges have the same start of end, one of the ranges in the ValueTuple will be Empty.</remarks>
    /// <exception cref="ArgumentNullException">If <paramref name="range"/> is null.</exception>
    /// <exception cref="ArgumentException">If <paramref name="range"/> does not overlap with the current <see cref="Range{T}"/></exception>
    public (Range<T>, Range<T>) Exclusive(Range<T> range)
    {
            // IDEA: Return an IEnumerable<Range<T>> instead of a Tuple, and omit Range<T>.Empty

#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(range);
#else
        if (range == null)
            throw new ArgumentNullException(nameof(range));
#endif

        if (Equals(range))
            throw new ArgumentException("There are no Exclusive ranges when both are equal.");

        // The specified range is completely part of the current range
        if (Includes(range))
        {
            return (new Range<T>(_start, range._start), new Range<T>(range._end, _end));
        }

        // The current range is completely part of the specified range
        if (range.Includes(this))
        {
            return (new Range<T>(range._start, _start), new Range<T>(_end, range._end));
        }

        // There is an overlap between the ranges
        if (Overlaps(range))
        {
            var start1 = _start.CompareTo(range._start) < 0 ? _start : range._start;
            var end1 = _start.CompareTo(range._start) > 0 ? _start : range._start;

            var start2 = _end.CompareTo(range._end) < 0 ? _end : range._end;
            var end2 = _end.CompareTo(range._end) > 0 ? _end : range._end;

            return (new Range<T>(start1, end1), new Range<T>(start2, end2));
        }

        // Both ranges have nothing in common, thus are already exclusive
        return (this, range);
    }

    // TODO: Add an EnumerateBy with a default step and/or stepper

    /// <summary>
    /// Enumerate all values that are part of the <see cref="Range{T}"/>, by a given step.
    /// The first element that is returned is always the start of the <see cref="Range{T}"/>,
    /// the following values are generated via a stepper function.
    /// It ends with the last element that is still part of the <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="step">Value to use</param>
    /// <param name="stepper">Generator function which takes the previous generated element and uses it to generate the next element.</param>
    /// <returns>An <see cref="IEnumerable{TRange}"></see> containing all values between start and end that can be generated by the stepper function</returns>
    protected internal IEnumerable<T> EnumerateBy(T step, Func<T, T, T> stepper)
    {
        return EnumerateBy<T>(step, stepper);
    }

    /// <summary>
    /// Enumerate all values that are part of the <see cref="Range{T}"/>, by a given step.
    /// The first element that is returned is always the start of the <see cref="Range{T}"/>,
    /// the following values are generated via a stepper function.
    /// It ends with the last element that is still part of the <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="step">Value to use</param>
    /// <param name="stepper">Generator function which takes the previous generated element and uses it to generate the next element.</param>
    /// <returns>An <see cref="IEnumerable{TRange}"></see> containing all values between start and end that can be generated by the stepper function</returns>
    protected internal IEnumerable<T> EnumerateBy<TStep>(TStep step, Func<T, TStep, T> stepper)
        where TStep : notnull
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(stepper);
#else
        if (step == null)
            throw new ArgumentNullException(nameof(step));
        if (stepper == null)
            throw new ArgumentNullException(nameof(stepper));
#endif

        if (IsEmpty())
            yield break;

        yield return _start; // Always return start

        var previousValue = _start;
        for (var value = stepper(_start, step); value.CompareTo(_end) < 0; value = stepper(value, step))
        {
            if (value.CompareTo(_start) < 0)
                throw new NotSupportedException(
                    $"Enumerating is not possible because the {value} is lower than the start of {this}.");
            if (value.CompareTo(previousValue) <= 0)
                throw new InvalidOperationException(
                    $"The stepper should create a higher value on every step. It has returned {value} for two times in a row.");

            previousValue = value;

            yield return value;
        }
    }

    /// <summary>
    /// Determines whether the specified <see cref="Range{T}"/> is equal to the current <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="other">The <see cref="Range{T}"/> to compare with the current object.</param>
    /// <returns>true if the specified <see cref="Range{T}"/> is equal to the current <see cref="Range{T}"/>; otherwise, false.</returns>
    public bool Equals(Range<T>? other)
    {
        // If parameter is null, return false.
        if (other is null)
            return false;

        // Optimization for a common success case.
        if (ReferenceEquals(this, other))
            return true;

        // If run-time types are not exactly the same, return false.
        if (GetType() != other.GetType())
            return false;

        if (IsEmpty() && other.IsEmpty())
            return true;

        return _start.Equals(other._start) && _end.Equals(other._end);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return Equals(obj as Range<T>);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
#if NETSTANDARD2_0
        // ReSharper disable twice ConditionIsAlwaysTrueOrFalse
        return ((_start == null ? 0 :_start.GetHashCode() << 2)) ^ (_end == null ? 0 : _end.GetHashCode());
#else
        // ReSharper disable twice ConditionIsAlwaysTrueOrFalse
        return HashCode.Combine(_start == null ? 0 : _start.GetHashCode(), _end == null ? 0 : _end.GetHashCode());
#endif
    }

    /// <inheritdoc />
    /// <remarks>When <typeparamref name="T"/> implements <see cref="ICloneable"/>, they are also cloned.</remarks>
    public object Clone()
    {
        var start = this._start;
        var end = this._end;

        if (start is ICloneable cloneableStart && end is ICloneable cloneableEnd)
        {
            start = (T) cloneableStart.Clone();
            end = (T) cloneableEnd.Clone();
        }

        return new Range<T>(start, end);
    }

    /// <inheritdoc />
    public int CompareTo(Range<T>? other)
    {
        // If other is not a valid object reference, this instance is greater
        if (other is null)
            return 1;

        // Both are empty, then they are the same
        if (IsEmpty() && other.IsEmpty())
            return 0;

        // If other is empty, then this instance is greater
        if (other.IsEmpty())
            return 1;

        // If current is empty, than this instance is lower
        if (IsEmpty())
            return -1;

        // First compare the Start, if they are the same compare the End
        var result = _start.CompareTo(other._start);
        if (result == 0)
            result = _end.CompareTo(other._end);

        return result;
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        return CompareTo(obj as Range<T>);
    }

    /// <summary>
    /// Determines whether the specified <see cref="Range{T}"/> is greater than the current <see cref="Range{T}"/>, but without touching or overlapping each other.
    /// </summary>
    /// <param name="other">The <see cref="Range{T}"/> to compare with the current object.</param>
    /// <returns>true if the specified <see cref="Range{T}"/> is lower than the current <see cref="Range{T}"/> and does not touch nor overlaps it; otherwise, false.</returns>
    public bool IsCompletelyBefore(Range<T>? other)
    {
        // If other is not a valid object reference, this instance is greater
        if (other is null)
            return false;

        // Both are empty, then they are the same
        if (IsEmpty() && other.IsEmpty())
            return false;

        // If other is empty, then this instance is greater
        if (other.IsEmpty())
            return false;

        // If current is empty, than this instance is lower
        if (IsEmpty())
            return true;

        return _end.CompareTo(other._start) < 0;
    }

    /// <summary>
    /// Determines whether the specified <see cref="Range{T}"/> is lower than the current <see cref="Range{T}"/>, but without touching or overlapping each other.
    /// </summary>
    /// <param name="other">The <see cref="Range{T}"/> to compare with the current object.</param>
    /// <returns>true if the specified <see cref="Range{T}"/> is greater than the current <see cref="Range{T}"/> and does not touch nor overlaps it; otherwise, false.</returns>
    public bool IsCompletelyBehind(Range<T>? other)
    {
        // If other is not a valid object reference, this instance is greater
        if (other is null)
            return true;

        // Both are empty, then they are the same
        if (IsEmpty() && other.IsEmpty())
            return false;

        // If other is empty, then this instance is greater
        if (other.IsEmpty())
            return true;

        // If current is empty, than this instance is lower
        if (IsEmpty())
            return false;

        return _start.CompareTo(other._end) > 0;
    }

    /// <summary>
    /// Returns the <see cref="Range{T}"/> as <see cref="ValueTuple{T, T}"/>
    /// </summary>
    /// <returns><see cref="ValueTuple{T, T}"/></returns>
    public ValueTuple<T, T> AsTuple()
    {
        return (_start, _end);
    }

    /// <summary>
    /// Determines whether two specified Ranges have the same value.
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
    /// <returns><c>true</c> if the <paramref name="leftRange"/> and <paramref name="rightRange"/> parameters have the same value; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Range<T>? leftRange, Range<T>? rightRange)
    {
        // Check for null
        if (leftRange is null)
        {
            if (rightRange is null)
            {
                // null == null => true
                return true;
            }

            // Only the left side is null
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
    public static bool operator !=(Range<T>? leftRange, Range<T>? rightRange)
    {
        return !(leftRange == rightRange);
    }

    /// <summary>
    /// Determines whether one specified <see cref="Range{T}"/> is greater than another specified <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
    /// <returns>true if <paramref name="leftRange" /> is greater than <paramref name="rightRange" />; otherwise, false.</returns>
    public static bool operator >(Range<T>? leftRange, Range<T>? rightRange)
    {
        if (leftRange == null)
            return false;

        return leftRange.CompareTo(rightRange) == 1;
    }

    /// <summary>
    /// Determines whether one specified <see cref="Range{T}"/> is lower than another specified <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
    /// <returns>true if <paramref name="leftRange" /> is lower than <paramref name="rightRange" />; otherwise, false.</returns>
    public static bool operator <(Range<T>? leftRange, Range<T>? rightRange)
    {
        if (leftRange == null)
            return true;

        return leftRange.CompareTo(rightRange) == -1;
    }

    /// <summary>
    /// Determines whether one specified <see cref="Range{T}"/> represents a range that is the same as or lower than another specified <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
    /// <returns>true if <paramref name="leftRange" /> is the same as or lower than <paramref name="rightRange" />; otherwise, false.</returns>
    public static bool operator >=(Range<T>? leftRange, Range<T>? rightRange)
    {
        if (leftRange == null)
            return false;

        return leftRange.CompareTo(rightRange) >= 0;
    }

    /// <summary>
    /// Determines whether one specified <see cref="Range{T}"/> represents a range that is the same as or greater than another specified <see cref="Range{T}"/>.
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/> to compare, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/> to compare, or null.</param>
    /// <returns>true if <paramref name="leftRange" /> is the same as or greater than <paramref name="rightRange" />; otherwise, false.</returns>
    public static bool operator <=(Range<T>? leftRange, Range<T>? rightRange)
    {
        if (leftRange == null)
            return true;

        return leftRange.CompareTo(rightRange) <= 0;
    }

    /// <summary>
    /// Performs a logical disjunction (OR) on two Ranges
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/> to merge, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/> to merge, or null.</param>
    /// <returns>The merged <see cref="Range{T}"/> of the two Ranges</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="leftRange"/> is null.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="rightRange"/> is null.</exception>
    public static Range<T> operator |(Range<T>? leftRange, Range<T>? rightRange)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(leftRange);
        ArgumentNullException.ThrowIfNull(rightRange);
#else
        if (leftRange == null)
            throw new ArgumentNullException(nameof(leftRange));
        if (rightRange == null)
            throw new ArgumentNullException(nameof(rightRange));
#endif

            return leftRange.Merge(rightRange);
    }

    /// <summary>
    /// Performs a logical conjunction (AND) on two Ranges
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/> to intersect, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/> to intersect, or null.</param>
    /// <returns>The intersection <see cref="Range{T}"/> of the two Ranges</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="leftRange"/> is null.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="rightRange"/> is null.</exception>
    public static Range<T> operator &(Range<T> leftRange, Range<T> rightRange)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(leftRange);
        ArgumentNullException.ThrowIfNull(rightRange);
#else
            if (leftRange == null)
            throw new ArgumentNullException(nameof(leftRange));
        if (rightRange == null)
            throw new ArgumentNullException(nameof(rightRange));
#endif

        return leftRange.Intersection(rightRange);
    }

    /// <summary>
    /// Performs an Exclusive or (XOR) on two Ranges
    /// </summary>
    /// <param name="leftRange">The first <see cref="Range{T}"/>, or null.</param>
    /// <param name="rightRange">The second <see cref="Range{T}"/>, or null.</param>
    /// <returns>A ValueTuple of the two exclusive Ranges</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="leftRange"/> is null.</exception>
    /// <exception cref="ArgumentNullException">If <paramref name="rightRange"/> is null.</exception>
    public static (Range<T>, Range<T>) operator ^(Range<T> leftRange, Range<T> rightRange)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(leftRange);
        ArgumentNullException.ThrowIfNull(rightRange);
#else
            if (leftRange == null)
            throw new ArgumentNullException(nameof(leftRange));
        if (rightRange == null)
            throw new ArgumentNullException(nameof(rightRange));
#endif

        return leftRange.Exclusive(rightRange);
    }

    /// <summary>
    /// Implicitly converts a <see cref="Range{T}"/> to a <see cref="ValueTuple{T, T}"/>
    /// </summary>
    /// <param name="range"><see cref="Range{T}"/> to convert</param>
    public static implicit operator ValueTuple<T, T>(Range<T> range)
    {
#if NET8_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(range);
#else
        if (range == null)
            throw new ArgumentNullException(nameof(range));
#endif

        return range.AsTuple();
    }

    /// <summary>
    /// Explicitly converts a <see cref="ValueTuple{T, T}"/> to a <see cref="Range{T}"/>
    /// </summary>
    /// <param name="tuple"><see cref="ValueTuple{T, T}"/> to convert</param>
    /// <remarks>Explicit because not every <see cref="ValueTuple{T, T}"/> can safely be converted to a <see cref="Range{T}"/></remarks>
    public static explicit operator Range<T>(ValueTuple<T, T> tuple)
    {
        return new Range<T>(tuple);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return this != Empty ? $"Range({_start}, {_end})" : "Range.Empty";
    }
    }
}