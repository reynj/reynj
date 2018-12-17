using System;
using FluentAssertions;
using Xunit;

namespace Reynj.UnitTests
{
    public class RangeTests
    {
        [Theory]
        [InlineData(10, 9)]
        public void Ctor_EndMustBeLowerThanOrEqualToStart(int start, int end)
        {
            // Arrange - Act
            Action act = () => new Range<int>(start, end);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("end must be greater than or equal to start");
        }

        [Fact]
        public void Ctor_StartAndEndPropertyAreSet()
        {
            // Arrange - Act
            var range = new Range<int>(1, 99);

            // Assert
            range.Start.Should().Be(1);
            range.End.Should().Be(99);
        }

        [Fact]
        public void Equals_IsFalse_WhenOtherIsNull()
        {
            // Arrange
            var range = new Range<int>(1, 99);

            // Act - Assert
            range.Equals(null).Should().BeFalse();
            range.Equals((object) null).Should().BeFalse();
        }

        [Fact]
        public void Equals_IsTrue_WhenOtherHasSameReference()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = range;

            // Act - Assert
            range.Equals(otherRange).Should().BeTrue();
            range.Equals((object) otherRange).Should().BeTrue();
        }

        [Fact]
        public void Equals_IsFalse_WhenTypesDoNotMatchExactly()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var numericRange = new NumericRange(1, 99);

            // Act - Assert
            range.Equals(numericRange).Should().BeFalse();
            range.Equals((object) numericRange).Should().BeFalse();
        }

        private class NumericRange : Range<int>
        {
            public NumericRange(int start, int end) : base(start, end)
            {
            }
        }

        [Fact]
        public void Equals_IsTrue_WhenStartAndEndAreEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            range.Equals(otherRange).Should().BeTrue();
            range.Equals((object) otherRange).Should().BeTrue();
        }

        [Fact]
        public void GetHashCode_IsSame_WhenStartAndEndAreEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            range.GetHashCode().Should().Be(otherRange.GetHashCode());
        }

        [Fact]
        public void ComparisonOperator_Equals_IsTrue_WhenPeriodsAreEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            (range == otherRange).Should().BeTrue();
        }

        [Fact]
        public void ComparisonOperator_Equals_IsTrue_WhenBothPeriodsAreNull()
        {
            // Arrange
            var range = (Range<int>) null;
            var otherRange = (Range<int>) null;

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range == otherRange).Should().BeTrue();
        }

        [Fact]
        public void ComparisonOperator_Equals_IsFalse_WhenOnePeriodIsNull()
        {
            // Arrange
            var range = (Range<int>) null;
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range == otherRange).Should().BeFalse();
        }

        [Fact]
        public void ComparisonOperator_Equals_IsFalse_WhenTheOtherPeriodIsNull()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = (Range<int>) null;

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range == otherRange).Should().BeFalse();
        }

        [Fact]
        public void ComparisonOperator_Equals_IsFalse_WhenPeriodsAreNotEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(2, 100);

            // Act - Assert
            (range == otherRange).Should().BeFalse();
        }

        [Fact]
        public void ToString_ReturnsAnInformalStringDescribingTheRange()
        {
            // Arrange
            var range = new Range<int>(1, 99);

            // Act - Assert
            range.ToString().Should().Be("Range(1, 99)");
        }
    }
}
