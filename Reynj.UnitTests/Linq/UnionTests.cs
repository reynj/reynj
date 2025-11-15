using Reynj.Linq;

namespace Reynj.UnitTests.Linq
{
    public class UnionTests
    {
        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
        public void Union_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = Array.Empty<Range<int>>();

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Union(null).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("second");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
        public void Union_AsNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Union(Array.Empty<Range<int>>()).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("first");
        }

        [Theory]
        [MemberData(nameof(UnionData))]
        public void Union_ReturnsTheExpectedResult(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second,
            IEnumerable<Range<int>> expectedUnion)
        {
            // Act
            var unionOf = first.Union(second);

            // Assert
            unionOf.Should().BeEquivalentTo(expectedUnion);
        }

        [Theory]
        [MemberData(nameof(UnionData))]
        public void Union_ReturnsTheExpectedResult_OtherWayAround(IEnumerable<Range<int>> first,
            IEnumerable<Range<int>> second, IEnumerable<Range<int>> expectedUnion)
        {
            // Act
            var unionOf = second.Union(first);

            // Assert
            unionOf.Should().BeEquivalentTo(expectedUnion);
        }

        public static IEnumerable<object[]> UnionData()
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(20, 30)
                })
            };

            // Complex
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 5),
                    new Range<int>(3, 10),
                    new Range<int>(10, 15)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(15, 17),
                    new Range<int>(18, 25)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 17),
                    new Range<int>(18, 25)
                })
            };
        }
    }
}