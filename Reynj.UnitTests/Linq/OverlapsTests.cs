using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Reynj.Linq;
using Xunit;

namespace Reynj.UnitTests.Linq
{
    public class OverlapsTests
    {
        [Fact]
        public void Overlaps_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = new Range<int>[] { };

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Overlaps(null).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("second");
        }

        [Fact]
        public void Overlaps_AsNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Overlaps(new Range<int>[] { }).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("first");
        }

        [Theory]
        [MemberData(nameof(OverlapsData))]
        public void Overlaps_ReturnsTheExpectedResult(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second,
            bool expectedOverlaps)
        {
            // Act
            var overlaps = first.Overlaps(second);

            // Assert
            intersected.Should().Be(expectedOverlaps);
        }

        [Theory]
        [MemberData(nameof(OverlapsData))]
        public void Overlaps_ReturnsTheExpectedResult_OtherWayAround(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second, bool expectedOverlaps)
        {
            // Act
            var intersected = second.Overlaps(first);

            // Assert
            intersected.Should().Be(expectedOverlaps);
        }

        public static IEnumerable<object[]> OverlapsData()
        {
            // Empty Lists
            yield return new object[]
            {
                new List<Range<int>>(),
                new List<Range<int>>(),
                false
            };

            // A single Range that is the same
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                true
            };

            // An empty Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    Range<int>.Empty
                }),
                new List<Range<int>>(),
                false
            };

            // An empty Range combined with a single Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    Range<int>.Empty,
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                true
            };

            // Two touching Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(10, 20)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20)
                }),
                false
            };

            // Included in the other Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20),
                    new Range<int>(5, 15)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20)
                }),
                true
            };

            // Non-overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(20, 30)
                }),
                false
            };

           // Complex
           yield return new object[]
           {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 5),
                    new Range<int>(3, 10),
                    new Range<int>(10, 15),
                    new Range<int>(18, 20)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(1, 8),
                    new Range<int>(12, 25)
                }),
                true
           };

            // More Complex
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 5),
                    new Range<int>(10, 15),
                    new Range<int>(20, 25)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(-5, -2),
                    new Range<int>(2, 7),
                    new Range<int>(12, 17),
                    new Range<int>(22, 27),
                    new Range<int>(32, 37)
                }),
                true
            };
        }
    }
}