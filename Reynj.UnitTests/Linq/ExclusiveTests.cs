using System;
using System.Collections.Generic;
using System.Linq;
using Reynj.Linq;

namespace Reynj.UnitTests.Linq
{
    public class ExclusiveTests
    {
        [Fact]
        public void Exclusive_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = new Range<int>[] { };

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Exclusive(null).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("second");
        }

        [Fact]
        public void Exclusive_AsNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Exclusive(new Range<int>[] { }).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("first");
        }

        [Theory]
        [MemberData(nameof(ExclusiveData))]
        public void Exclusive_ReturnsTheExpectedResult(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second, IEnumerable<Range<int>> expectedExclusiveOf)
        {
            // Act
            var exclusiveOf = first.Exclusive(second);

            // Assert
            exclusiveOf.Should().BeEquivalentTo(expectedExclusiveOf);
        }

        [Theory]
        [MemberData(nameof(ExclusiveData))]
        public void Exclusive_ReturnsTheExpectedResult_OtherWayAround(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second, IEnumerable<Range<int>> expectedExclusiveOf)
        {
            // Act
            var exclusiveOf = second.Exclusive(first);

            // Assert
            exclusiveOf.Should().BeEquivalentTo(expectedExclusiveOf);
        }

        public static IEnumerable<object[]> ExclusiveData()
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