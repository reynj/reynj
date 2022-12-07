#if !NETCOREAPP2_1
using FluentAssertions;
using Xunit;
using Reynj.Extensions;

namespace Reynj.UnitTests.Extensions
{
    public class RangeExtensionsTests
    {
        [Theory]
        [MemberData(nameof(RangeData))]
        public void ToRange_OnASystemRange_ReturnsAReynjRangeOfTypeInt(Range systemRange, Range<int> reynjRange)
        {
            // Arrange
            // Act
            var range = systemRange.ToRange();

            // Assert
            range.Should().Be(reynjRange);
        }

        [Theory]
        [MemberData(nameof(RangeData))]
        public void ToRange_OnAReynjRangeOfTypeInt_ReturnsASystemRange(Range systemRange, Range<int> reynjRange)
        {
            // Arrange
            // Act
            var range = reynjRange.ToRange();

            // Assert
            range.Should().Be(systemRange);
        }

        public static IEnumerable<object[]> RangeData()
        {
            yield return new object[] { new Range(0, 4), new Range<int>(0, 4) };

            yield return new object[] { Range.StartAt(0), new Range<int>(~0, 0) };
            yield return new object[] { Range.EndAt(4), new Range<int>(0, 4) };

            yield return new object[] { ..4, new Range<int>(0, 4) };
            yield return new object[] { 1..4, new Range<int>(1, 4) };
            yield return new object[] { ..^4, new Range<int>(~4, 0) };
            yield return new object[] { ^4.., new Range<int>(~4, ~0) };
            yield return new object[] { .., new Range<int>(~0, 0) };

            yield return new object[] { 1..^2, new Range<int>(~2, 1) };
        }

        [Fact]
        public void ToRange_CanBeUsedToMergeSystemRanges()
        {
            // Arrange
            var sysRange1 = ..10;
            var sysRange2 = 5..15;

            var range1 = sysRange1.ToRange();
            var range2 = sysRange2.ToRange();

            // Act
            var merged = range1.Merge(range2);
            var sysMerged = merged.ToRange();

            // Assert
            sysMerged.Should().Be(..15);
        }
    }
}
#endif