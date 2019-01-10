using System;
using System.Collections.Generic;
using FluentAssertions;
using Reynj.Linq;
using Xunit;

namespace Reynj.UnitTests.Linq
{
    public class HighestTests
    {
        [Fact]
        public void Highest_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => ranges.Highest();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Fact]
        public void Highest_WithAnEmptyList_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = new List<Range<int>>();

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => ranges.Highest();

            // Assert
            act.Should().Throw<NotSupportedException>()
                .And.Message.Should().Be("Highest is not supported on an empty collection.");
        }
        
        [Fact]
        public void Highest_ReturnsTheLowestStartOfAllTheRanges()
        {
            // Arrange
            var ranges = new List<Range<int>>
            {
                new Range<int>(0, 10),
                new Range<int>(10, 20),
                new Range<int>(10, 20),
                new Range<int>(-10, 0)
            };

            // Act
            var lowest = ranges.Highest();

            // Assert
            lowest.Should().Be(20);
        }
    }
}