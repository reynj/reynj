using Reynj.Linq;

namespace Reynj.UnitTests.Linq
{
    public class LowestTests
    {
        [Fact]
        public void Lowest_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => ranges.Lowest();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Fact]
        public void Lowest_WithAnEmptyList_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = new List<Range<int>>();

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            Action act = () => ranges.Lowest();

            // Assert
            act.Should().Throw<NotSupportedException>()
                .And.Message.Should().Be("Lowest is not supported on an empty collection.");
        }
        
        [Fact]
        public void Lowest_ReturnsTheLowestStartOfAllTheRanges()
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
            var lowest = ranges.Lowest();

            // Assert
            lowest.Should().Be(-10);
        }
    }
}