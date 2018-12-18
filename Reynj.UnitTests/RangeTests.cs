using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Reynj.UnitTests
{
    public class RangeTests
    {
        [Theory]
        [InlineData(10, 9)]
        public void Ctor_EndMustBeLessThanOrEqualToStart(int start, int end)
        {
            // Arrange - Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Range<int>(start, end);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("end must be greater than or equal to start");
        }

        [Fact]
        public void Ctor_StartAndEndPropertyAreSet()
        {
            // Arrange
            var range = new Range<int>(1, 99);

            // Act - Assert
            range.Start.Should().Be(1);
            range.End.Should().Be(99);
        }

        [Fact]
        public void Includes_ReturnsFalse_GivenNullAsValue()
        {
            // Arrange 
            var range = new Range<string>("a", "z");

            // Act - Assert
            range.Includes(null).Should().BeFalse();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(5)]
        [InlineData(9)]
        public void Includes_ReturnsTrue_GivenAValueThatIsPartOfTheRange(int value)
        {
            // Arrange 
            var range = new Range<int>(0, 10);

            // Act - Assert
            range.Includes(value).Should().BeTrue();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(-99)]
        [InlineData(99)]
        public void Includes_ReturnsFalse_GivenAValueThatIsNotPartOfTheRange(int value)
        {
            // Arrange 
            var range = new Range<int>(0, 10);

            // Act - Assert
            range.Includes(value).Should().BeFalse();
        }

        [Fact]
        public void IncludesAll_ReturnsTrue_GivenAllValuesArePartOfTheRange()
        {
            // Arrange 
            var range = new Range<int>(0, 10);

            // Act - Assert
            range.IncludesAll(0, 1, 2, 3, 4, 5, 6, 7, 8, 9).Should().BeTrue();
            range.IncludesAll(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).Should().BeTrue();
        }

        [Fact]
        public void IncludesAll_ReturnsFalse_GivenOneOfTheValuesAreNotPartOfTheRange()
        {
            // Arrange 
            var range = new Range<int>(0, 10);

            // Act - Assert
            range.IncludesAll(0, 1, 2, 3, 4, 99, 6, 7, 8, 9).Should().BeFalse();
            range.IncludesAll(new List<int> {0, 1, 2, 3, 4, 99, 6, 7, 8, 9 }).Should().BeFalse();
        }

        [Fact]
        public void IsEmpty_ReturnsFalse_WhenStartAndEndAreNotEqual()
        {
            // Arrange 
            var range = new Range<int>(0, 10);

            // Act - Assert
            range.IsEmpty().Should().BeFalse();
        }

        [Fact]
        public void IsEmpty_ReturnsTrue_WhenStartAndEndAreEqual()
        {
            // Arrange 
            var range = new Range<int>(10, 10);

            // Act - Assert
            range.IsEmpty().Should().BeTrue();
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
        public void CompareTo_IsOne_WhenOtherIsNull()
        {
            // Arrange
            var range = new Range<int>(1, 99);

            // Act - Assert
            range.CompareTo(null).Should().Be(1);
            range.CompareTo((object) null).Should().Be(1);
        }

        [Fact]
        public void CompareTo_IsZero_WhenRangesAreEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            range.CompareTo(otherRange).Should().Be(0);
            range.CompareTo((object) otherRange).Should().Be(0);
        }

        [Fact]
        public void CompareTo_IsMinusOne_WhenOtherHasAHigherStart()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(2, 99);

            // Act - Assert
            range.CompareTo(otherRange).Should().Be(-1);
            range.CompareTo((object) otherRange).Should().Be(-1);
        }

        [Fact]
        public void CompareTo_IsOne_WhenOtherHasALowerStart()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(-2, 99);

            // Act - Assert
            range.CompareTo(otherRange).Should().Be(1);
            range.CompareTo((object) otherRange).Should().Be(1);
        }

        [Fact]
        public void CompareTo_IsMinusOne_WhenStartIsSameAndOtherHasAHigherEnd()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 100);

            // Act - Assert
            range.CompareTo(otherRange).Should().Be(-1);
            range.CompareTo((object) otherRange).Should().Be(-1);
        }

        [Fact]
        public void CompareTo_IsOne_WhenStartIsSameAndOtherHasALowerEnd()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 98);

            // Act - Assert
            range.CompareTo(otherRange).Should().Be(1);
            range.CompareTo((object) otherRange).Should().Be(1);
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
        public void EqualityOperator_IsTrue_WhenRangesAreEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            (range == otherRange).Should().BeTrue();
        }

        [Fact]
        public void EqualityOperator_IsTrue_WhenBothRangesAreNull()
        {
            // Arrange
            var range = (Range<int>) null;
            var otherRange = (Range<int>) null;

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range == otherRange).Should().BeTrue();
        }

        [Fact]
        public void EqualityOperator_IsFalse_WhenOneRangeIsNull()
        {
            // Arrange
            var range = (Range<int>) null;
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range == otherRange).Should().BeFalse();
        }

        [Fact]
        public void EqualityOperator_IsFalse_WhenTheOtherRangeIsNull()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = (Range<int>) null;

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range == otherRange).Should().BeFalse();
        }

        [Fact]
        public void EqualityOperator_IsFalse_WhenRangesAreNotEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(2, 100);

            // Act - Assert
            (range == otherRange).Should().BeFalse();
        }

        [Fact]
        public void InequalityOperator_IsFalse_WhenRangesAreEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            (range != otherRange).Should().BeFalse();
        }

        [Fact]
        public void InequalityOperator_IsFalse_WhenBothRangesAreNull()
        {
            // Arrange
            var range = (Range<int>) null;
            var otherRange = (Range<int>) null;

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range != otherRange).Should().BeFalse();
        }

        [Fact]
        public void InequalityOperator_IsTrue_WhenOneRangeIsNull()
        {
            // Arrange
            var range = (Range<int>) null;
            var otherRange = new Range<int>(1, 99);

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range != otherRange).Should().BeTrue();
        }

        [Fact]
        public void InequalityOperator_IsTrue_WhenTheOtherRangeIsNull()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = (Range<int>) null;

            // Act - Assert
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            (range != otherRange).Should().BeTrue();
        }

        [Fact]
        public void InequalityOperator_IsTrue_WhenRangesAreNotEqual()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var otherRange = new Range<int>(2, 100);

            // Act - Assert
            (range != otherRange).Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GreaterThanOperatorData))]
        public void GreaterThanOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            (range > otherRange).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> GreaterThanOperatorData()
        {
            // Both are the same
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 99), false};

            // First is greater than
            yield return new object[] {new Range<int>(2, 99), new Range<int>(1, 99), true};
            yield return new object[] {new Range<int>(1, 100), new Range<int>(1, 99), true};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(-99, -1), true};

            // First is lower than
            yield return new object[] {new Range<int>(1, 99), new Range<int>(2, 99), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 100), false};
            yield return new object[] {new Range<int>(-99, -1), new Range<int>(1, 99), false};
        }

        [Theory]
        [MemberData(nameof(LessThanOperatorData))]
        public void LessThanOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            (range < otherRange).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> LessThanOperatorData()
        {
            // Both are the same
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 99), false};

            // First is less than
            yield return new object[] {new Range<int>(1, 99), new Range<int>(2, 99), true};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 100), true};
            yield return new object[] {new Range<int>(-99, -1), new Range<int>(1, 99), true};

            // First is greater than
            yield return new object[] {new Range<int>(2, 99), new Range<int>(1, 99), false};
            yield return new object[] {new Range<int>(1, 100), new Range<int>(1, 99), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(-99, -1), false};
        }

        [Theory]
        [MemberData(nameof(GreaterThanOrEqualOperatorData))]
        public void GreaterThanOrEqualOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            (range >= otherRange).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> GreaterThanOrEqualOperatorData()
        {
            // Both are the same
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 99), true};

            // First is greater than
            yield return new object[] {new Range<int>(2, 99), new Range<int>(1, 99), true};
            yield return new object[] {new Range<int>(1, 100), new Range<int>(1, 99), true};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(-99, -1), true};

            // First is lower than
            yield return new object[] {new Range<int>(1, 99), new Range<int>(2, 99), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 100), false};
            yield return new object[] {new Range<int>(-99, -1), new Range<int>(1, 99), false};
        }

        [Theory]
        [MemberData(nameof(LessThanOrEqualOperatorData))]
        public void LessThanOrEqualOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            (range <= otherRange).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> LessThanOrEqualOperatorData()
        {
            // Both are the same
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 99), true};

            // First is less than
            yield return new object[] {new Range<int>(1, 99), new Range<int>(2, 99), true};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 100), true};
            yield return new object[] {new Range<int>(-99, -1), new Range<int>(1, 99), true};

            // First is greater than
            yield return new object[] {new Range<int>(2, 99), new Range<int>(1, 99), false};
            yield return new object[] {new Range<int>(1, 100), new Range<int>(1, 99), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(-99, -1), false};
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
