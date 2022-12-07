using FluentAssertions;
using Reynj.Linq;
using Xunit;

namespace Reynj.UnitTests.Linq
{
    public class IntersectTests
    {
        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "Specific unit test")]
        public void Intersect_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = Array.Empty<Range<int>>();

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Intersect(null).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("second");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "Specific unit test")]
        public void Intersect_AsNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Intersect(Array.Empty<Range<int>>()).ToList();

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
        public void Intersect_ReturnsTheExpectedResult_OtherWayAround(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second, IEnumerable<Range<int>> expectedUnion)
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(2, 5),
                    new Range<int>(12, 15),
                    new Range<int>(22, 25)
                })
            };
        }
    }
}