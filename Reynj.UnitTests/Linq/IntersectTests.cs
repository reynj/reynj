using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Reynj.Linq;
using Xunit;

namespace Reynj.UnitTests.Linq
{
    public class IntersectTests
    {
        [Fact]
        public void Intersect_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = new Range<int>[] { };

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Intersect(null).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("second");
        }

        [Fact]
        public void Intersect_AsNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Intersect(new Range<int>[] { }).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("first");
        }

        [Theory]
        [MemberData(nameof(IntersectData))]
        public void Intersect_ReturnsTheExpectedResult(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second,
            IEnumerable<Range<int>> expectedUnion)
        {
            // Act
            var intersected = first.Intersect(second);

            // Assert
            intersected.Should().BeEquivalentTo(expectedUnion);
        }

        [Theory]
        [MemberData(nameof(IntersectData))]
        public void Intersect_ReturnsTheExpectedResult_OtherWayAround(IEnumerable<Range<int>> first,
            IEnumerable<Range<int>> second, IEnumerable<Range<int>> expectedUnion)
        {
            // Act
            var intersected = second.Intersect(first);

            // Assert
            intersected.Should().BeEquivalentTo(expectedUnion);
        }

        public static IEnumerable<object[]> IntersectData()
        {
            // Empty Lists
            yield return new object[]
            {
                new List<Range<int>>(),
                new List<Range<int>>(),
                new List<Range<int>>()
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                })
            };

            // An empty Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    Range<int>.Empty
                }),
                new List<Range<int>>(),
                new List<Range<int>>()
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                })
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20)
                })
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20)
                })
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
                new List<Range<int>>()
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(1, 8),
                    new Range<int>(12, 15),
                    new Range<int>(18, 20)
                })
            };
        }
    }
}