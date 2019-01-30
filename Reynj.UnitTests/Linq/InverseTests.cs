using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Reynj.Linq;
using Xunit;

namespace Reynj.UnitTests.Linq
{
    public class InverseTests
    {
        [Fact]
        public void Inverse_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Inverse().ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Theory]
        [MemberData(nameof(InverseData))]
        public void Inverse_ReturnsTheExpectedResult(IEnumerable<Range<int>> ranges, IEnumerable<Range<int>> expectedInversed)
        {
            // Act
            var reduced = ranges.Inverse();

            // Assert
            reduced.Should().BeEquivalentTo(expectedInversed);
        }

        [Theory]
        [MemberData(nameof(InverseData))]
        public void Inverse_ReturnsTheExpectedResult_AlsoForReversedLists(IEnumerable<Range<int>> ranges, IEnumerable<Range<int>> expectedInversed)
        {
            // Act
            var reduced = ranges.Inverse().Reduce();

            // Assert
            reduced.Should().BeEquivalentTo(expectedInversed);
        }

        public static IEnumerable<object[]> InverseData()
        {
            // Empty List
            yield return new object[]
            {
                new List<Range<int>>(),
                new List<Range<int>>
                {
                    new Range<int>(int.MinValue, int.MaxValue)
                }
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(10, int.MaxValue)
                })
            };

            // Range that goes from MinValue to MaxValue
            yield return new object[]
            {
                new List<Range<int>>
                {
                    new Range<int>(int.MinValue, int.MaxValue)
                },
                new List<Range<int>>()
            };

            // An empty Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    Range<int>.Empty
                }),
                new List<Range<int>>
                {
                    new Range<int>(int.MinValue, int.MaxValue)
                }
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(10, int.MaxValue)
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(20, int.MaxValue)
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(15, int.MaxValue)
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(20, int.MaxValue)
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(10, 20),
                    new Range<int>(30, int.MaxValue)
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
                    new Range<int>(int.MinValue, -5),
                    new Range<int>(1, 2),
                    new Range<int>(20, int.MaxValue)
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(9, 17),
                    new Range<int>(41, 50),
                    new Range<int>(55, int.MaxValue)
                })
            };

            // Complex with a MinValue
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
                    new Range<int>(25, 41),
                    new Range<int>(int.MinValue, -10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(-10, 0),
                    new Range<int>(9, 17),
                    new Range<int>(41, 50),
                    new Range<int>(55, int.MaxValue)
                })
            };

            // Complex with a MaxValue
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
                    new Range<int>(25, 41),
                    new Range<int>(100, int.MaxValue)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(9, 17),
                    new Range<int>(41, 50),
                    new Range<int>(55, 100)
                })
            };
        }
    }
}