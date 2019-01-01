using System;
using System.Collections.Generic;
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
    }
}