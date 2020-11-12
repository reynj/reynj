namespace Reynj.Extensions
{
#if !NETSTANDARD2_0
    /// <summary>
    /// Extension methods to support conversion from and to <see cref="System.Range"/>
    /// </summary>
    public static class RangeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Range<int> ToRange(this System.Range range)
        {
            var start = range.Start.IsFromEnd ? ~range.Start.Value : range.Start.Value;
            var end = range.End.IsFromEnd ? ~range.End.Value : range.End.Value;

            return end < start ? new Range<int>(end, start) : new Range<int>(start, end);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static System.Range ToRange(this Range<int> range)
        {
            var start = range.Start < 0 ? ~range.Start : range.Start;
            var end = range.End < 0 ? ~range.End : range.End;

            var startIndex = new System.Index(start, range.Start < 0);
            var endIndex = new System.Index(end, range.End < 0);

            if (end <= start && !(startIndex.IsFromEnd && endIndex.IsFromEnd))
                return new System.Range(endIndex, startIndex);

            return new System.Range(startIndex, endIndex);
        }
    }
#endif
}