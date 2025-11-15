using Reynj.Linq;

namespace Reynj.UnitTests.Linq
{
    public class ReduceTests
    {
        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
        public void Reduce_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Reduce().ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Theory]
        [MemberData(nameof(ReduceData))]
        public void Reduce_ReturnsTheExpectedResult(IEnumerable<Range<int>> ranges, IEnumerable<Range<int>> expectedReduced)
        {
            // Act
            var reduced = ranges.Reduce();

            // Assert
            reduced.Should().BeEquivalentTo(expectedReduced);
        }

        [Theory]
        [MemberData(nameof(ReduceData))]
        public void Reduce_ReturnsTheExpectedResult_AlsoForReversedLists(IEnumerable<Range<int>> ranges, IEnumerable<Range<int>> expectedReduced)
        {
            // Act
            var reduced = ranges.Reverse().Reduce();

            // Assert
            reduced.Should().BeEquivalentTo(expectedReduced);
        }

        public static IEnumerable<object[]> ReduceData()
        {
            // Empty List
            yield return new object[]
            {
                new List<Range<int>>(),
                new List<Range<int>>()
            };

            // A single Range
            yield return new object[]
            {
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
                })
            };

            // Two overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(5, 15)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 15)
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
                })
            };

            // Non-overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(20, 30)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(20, 30)
                })
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(-5, 1),
                    new Range<int>(2, 20)
                })
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
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 9),
                    new Range<int>(17, 41),
                    new Range<int>(50, 55)
                })
            };
        }
    }
}