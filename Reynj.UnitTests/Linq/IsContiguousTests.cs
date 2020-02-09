using System;
using System.Collections.Generic;
using FluentAssertions;
using Reynj.Linq;
using Xunit;

namespace Reynj.UnitTests.Linq
{
    public class IsContiguousTests
    {
        [Fact]
        public void IsContiguous_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.IsContiguous();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Theory]
        [MemberData(nameof(IsContiguousData))]
        public void IsContiguous_ReturnsTheExpectedResult(IEnumerable<Range<int>> ranges, bool expectedIsContiguous)
        {
            // Act
            var isContiguous = ranges.IsContiguous();

            // Assert
            isContiguous.Should().Be(expectedIsContiguous);
        }

        public static IEnumerable<object[]> IsContiguousData()
        {
            // Empty List
            yield return new object[]
            {
                new List<Range<int>>(),
                false
            };

            // A single Range
            yield return new object[]
            {
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
                false
            };

            // Two touching Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(10, 20)
                }),
                true
            };

            // Two touching Ranges (not ordered)
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(10, 20),
                    new Range<int>(0, 10)
                }),
                true
            };

            // Two overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(5, 15)
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
                false
            };

            // Non-overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(20, 30)
                }),
                false
            };

            // Mixed case
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(2, 10),
                    new Range<int>(-5, 1),
                    new Range<int>(30, 30),
                    new Range<int>(5, 20)
                }),
                false
            };

            // Complex
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(50, 55),
                    new Range<int>(17, 25),
                    new Range<int>(3, 7),
                    new Range<int>(0, 1),
                    new Range<int>(2, 6),
                    new Range<int>(4, 9),
                    new Range<int>(27, 32),
                    new Range<int>(1, 7),
                    Range<int>.Empty,
                    new Range<int>(25, 41)
                }),
                false
            };

            // Large list with a single Empty
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 6),
                    Range<int>.Empty,
                    new Range<int>(6, 10),
                    new Range<int>(10, 25),
                    new Range<int>(25, 30),
                    new Range<int>(30, 43)
                }),
                false
            };

            // Large list
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 6),
                    new Range<int>(6, 10),
                    new Range<int>(10, 25),
                    new Range<int>(25, 30),
                    new Range<int>(30, 43)
                }),
                true
            };
        }
    }
}