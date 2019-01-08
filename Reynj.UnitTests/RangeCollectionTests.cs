using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Reynj.UnitTests
{
    public class RangeCollectionTests
    {
        [Fact]
        public void Ctor_WithoutParametersIsPossible()
        {
            // Arrange - Act
            // ReSharper disable once CollectionNeverUpdated.Local
            var rangeCollection = new RangeCollection<int>();

            // Act - Assert
            rangeCollection.Should().BeEmpty();
        }

        [Fact]
        public void Ctor_Ranges_AcceptsAnIEnumerableOfRange()
        {
            // Arrange
            var ranges = new List<Range<int>>
            {
                new Range<int>(0, 10),
                new Range<int>(10, 20)
            };

            // Act
            var rangeCollection = new RangeCollection<int>(ranges);

            // Act - Assert
            rangeCollection.Should().BeEquivalentTo(ranges);
        }

        [Fact]
        public void Ctor_Ranges_CannotBeNull()
        {
            // Arrange - Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new RangeCollection<int>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.Message.Should().StartWith("Value cannot be null");
        }

        [Fact]
        public void Lowest_ReturnsTheLowestStartOfAllTheRanges()
        {
            // Arrange
            var rangeCollection = new RangeCollection<int>(new List<Range<int>>
            {
                new Range<int>(0, 10),
                new Range<int>(10, 20),
                new Range<int>(10, 20),
                new Range<int>(-10, 0)
            });

            // Act
            var lowest = rangeCollection.Lowest();

            // Assert
            lowest.Should().Be(-10);
        }

        [Fact]
        public void Lowest_OnAnEmptyCollection_IsNotSupported()
        {
            // Arrange
            var rangeCollection = new RangeCollection<int>();

            // Act
            Action act = () => rangeCollection.Lowest();

            // Assert
            act.Should().Throw<NotSupportedException>()
                .And.Message.Should().StartWith("Lowest is not supported on an empty RangeCollection.");
        }

        [Fact]
        public void Highest_ReturnsTheHighestEndOfAllTheRanges()
        {
            // Arrange
            var rangeCollection = new RangeCollection<int>(new List<Range<int>>
            {
                new Range<int>(0, 10),
                new Range<int>(10, 20),
                new Range<int>(10, 20),
                new Range<int>(-10, 0)
            });

            // Act
            var highest = rangeCollection.Highest();

            // Assert
            highest.Should().Be(20);
        }

        [Fact]
        public void Highest_OnAnEmptyCollection_IsNotSupported()
        {
            // Arrange
            var rangeCollection = new RangeCollection<int>();

            // Act
            Action act = () => rangeCollection.Highest();

            // Assert
            act.Should().Throw<NotSupportedException>()
                .And.Message.Should().StartWith("Highest is not supported on an empty RangeCollection.");
        }

        [Theory]
        [MemberData(nameof(ReduceRangeCollectionData))]
        public void Reduce_ReturnsTheExpectedResult(RangeCollection<int> ranges, RangeCollection<int> expectedReduced)
        {
            // Act
            var reduced = ranges.Reduce();

            // Assert
            reduced.Should().BeEquivalentTo(expectedReduced);
        }

        [Theory]
        [MemberData(nameof(ReduceRangeCollectionData))]
        public void Reduce_ReturnsTheExpectedResult_AlsoForReversedLists(RangeCollection<int> ranges, RangeCollection<int> expectedReduced)
        {
            // Act
            var reduced = new RangeCollection<int>(ranges.Reverse()).Reduce();

            // Assert
            reduced.Should().BeEquivalentTo(expectedReduced);
        }

        public static IEnumerable<object[]> ReduceRangeCollectionData()
        {
            // Empty RangeCollection
            yield return new object[]
            {
                new RangeCollection<int>(),
                new RangeCollection<int>()
            };

            // A single Range
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 10)
                })
            };

            // An empty Range
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    Range<int>.Empty
                }),
                new RangeCollection<int>()
            };

            // An empty Range combined with a single Range
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    Range<int>.Empty,
                    new Range<int>(0, 10)
                }),
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 10)
                })
            };

            // Two touching Ranges
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(10, 20)
                }),
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 20)
                })
            };

            // Two overlapping Ranges
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(5, 15)
                }),
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 15)
                })
            };

            // Included in the other Range
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 20),
                    new Range<int>(5, 15)
                }),
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 20)
                })
            };

            // Non-overlapping Ranges
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(20, 30)
                }),
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(20, 30)
                })
            };

            // Mixed case
            yield return new object[]
            {
                new RangeCollection<int>(new[]
                {
                    new Range<int>(2, 10),
                    new Range<int>(-5, 1),
                    new Range<int>(30, 30),
                    new Range<int>(5, 20)
                }),
                new RangeCollection<int>(new[]
                {
                    new Range<int>(-5, 1),
                    new Range<int>(2, 20)
                })
            };

            // Complex
            yield return new object[]
            {
                new RangeCollection<int>(new[]
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
                new RangeCollection<int>(new[]
                {
                    new Range<int>(0, 9),
                    new Range<int>(17, 41),
                    new Range<int>(50, 55)
                })
            };
        }
    }
}