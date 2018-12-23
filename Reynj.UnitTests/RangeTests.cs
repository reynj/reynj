using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Reynj.UnitTests
{
    public class RangeTests
    {
        [Fact]
        public void Empty_IsARangeWithStartAndEndEqualToDefault_ValueType()
        {
            // Arrange
            var empty = Range<int>.Empty;

            // Act - Assert
            empty.Start.Should().Be(default);
            empty.End.Should().Be(default);
        }

        [Fact]
        public void Empty_IsARangeWithStartAndEndEqualToDefault_ReferenceType()
        {
            // Arrange
            var empty = Range<Version>.Empty;

            // Act - Assert
            empty.Start.Should().Be(default);
            empty.End.Should().Be(default);
        }

        [Fact]
        public void Empty_IsEqualToAnotherEmpty()
        {
            // Arrange
            var empty1 = Range<int>.Empty;
            var empty2 = Range<int>.Empty;

            // Act - Assert
            empty1.Should().Be(empty2);
        }

        [Fact]
        public void Ctor_Start_CannotBeNull()
        {
            // Arrange - Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Range<string>(null, "");

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("start");
        }

        [Fact]
        public void Ctor_StartEnd_EndMustBeLessThanOrEqualToStart()
        {
            // Arrange - Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Range<int>(99, 1);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("end must be greater than or equal to start");
        }

        [Fact]
        public void Ctor_StartEnd_StartAndEndPropertyAreSet()
        {
            // Arrange
            var range = new Range<int>(1, 99);

            // Act - Assert
            range.Start.Should().Be(1);
            range.End.Should().Be(99);
        }

        [Fact]
        public void Ctor_Tuple_Item1CannotBeNull()
        {
            // Arrange
            var tuple = ((string) null, "2");
            
            // Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Range<string>(tuple);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("start");
        }

        [Fact]
        public void Ctor_Tuple_EndMustBeLessThanOrEqualToStart()
        {
            // Arrange - Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Range<int>((99, 1));

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("end must be greater than or equal to start");
        }

        [Fact]
        public void Ctor_Tuple_StartAndEndPropertyAreSet()
        {
            // Arrange
            var range = new Range<int>((1, 99));

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
            range.Includes((string) null).Should().BeFalse();
            range.Includes((Range<string>) null).Should().BeFalse();
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

        [Theory]
        [MemberData(nameof(IncludesRangeData))]
        public void Includes_ForRange_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            range.Includes(otherRange).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> IncludesRangeData()
        {
            // Both are the same
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 99), true};

            // Includes
            yield return new object[] {new Range<int>(1, 99), new Range<int>(2, 99), true};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(10, 49), true};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 98), true};

            // Does not include
            yield return new object[] {new Range<int>(1, 99), new Range<int>(1, 100), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(0, 99), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(-99, -1), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(0, 50), false};
            yield return new object[] {new Range<int>(1, 99), new Range<int>(50, 100), false};
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

        [Theory]
        [MemberData(nameof(OverlapsRangeData))]
        public void Overlaps_ForRange_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            range.Overlaps(otherRange).Should().Be(expectedResult);
            otherRange.Overlaps(range).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> OverlapsRangeData()
        {
            // Both are the same and thus overlap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(0, 10), true};

            // Overlaps
            yield return new object[] {new Range<int>(0, 10), new Range<int>(5, 10), true};
            yield return new object[] {new Range<int>(0, 10), new Range<int>(9, 20), true};
            yield return new object[] {new Range<int>(0, 10), new Range<int>(-9, 1), true};

            // Does not overlap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(10, 20), false};
            yield return new object[] {new Range<int>(0, 10), new Range<int>(15, 25), false};
            yield return new object[] {new Range<int>(0, 10), new Range<int>(-10, 0), false};
        }

        [Theory]
        [MemberData(nameof(TouchesRangeData))]
        public void Touches_ForRange_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            range.Touches(otherRange).Should().Be(expectedResult);
            otherRange.Touches(range).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> TouchesRangeData()
        {
            // Both are the same and thus do not touch
            yield return new object[] {new Range<int>(0, 10), new Range<int>(0, 10), false};

            // Do not touch because they overlap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(5, 10), false};
            yield return new object[] {new Range<int>(0, 10), new Range<int>(9, 20), false};
            yield return new object[] {new Range<int>(0, 10), new Range<int>(-9, 1), false};

            // Do not touch because they have a gap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(15, 25), false};

            // Touch
            yield return new object[] {new Range<int>(0, 10), new Range<int>(10, 20), true};
            yield return new object[] {new Range<int>(10, 20), new Range<int>(0, 10), true};
        }

        [Theory]
        [MemberData(nameof(GapRangeData))]
        public void Gap_ReturnsARange_ThatRepresentsTheGapBetweenTwoRanges(Range<int> range, Range<int> otherRange, Range<int> expectedGap)
        {
            // Act - Assert
            range.Gap(otherRange).Should().Be(expectedGap);
            otherRange.Gap(range).Should().Be(expectedGap);
        }

        public static IEnumerable<object[]> GapRangeData()
        {
            // Both are the same and thus no gap
            yield return new object[] { new Range<int>(0, 10), new Range<int>(0, 10), Range<int>.Empty };

            // Both overlap and thus no gap
            yield return new object[] { new Range<int>(0, 10), new Range<int>(5, 15), Range<int>.Empty };

            // Both touch each other, no gap
            yield return new object[] { new Range<int>(0, 10), new Range<int>(10, 20), Range<int>.Empty };

            // Gap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(20, 30), new Range<int>(10, 20)};
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
        public void AsTuple_ReturnsThePeriodAsValueTuple()
        {
            // Arrange
            var range = new Range<int>(1, 99);

            // Act
            var tuple = range.AsTuple();

            // Assert
            tuple.Should().Be((1, 99));
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
        public void ExplicitConversionOperator_FromTupleToRange()
        {
            // Arrange
            var tuple = (1, 10);

            // Act
            var range = (Range<int>) tuple;
            
            // - Assert
            range.Should().Be(new Range<int>(1, 10));
        }

        [Fact]
        public void ExplicitConversionOperator_FromTupleToRangeWithItem1GreaterThanItem2_ThrowsAnArgumentException()
        {
            // Arrange - Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () =>
            {
                // ReSharper disable once UnusedVariable
                var range = (Range<int>) (99, 1);
            };

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("end must be greater than or equal to start");
        }

        [Fact]
        public void ImplicitConversionOperator_FromRangeToTuple()
        {
            // Arrange
            var range = new Range<int>(1, 10);

            // Act
            ValueTuple<int, int> tuple = range;
            
            // - Assert
            tuple.Should().Be((1, 10));
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
