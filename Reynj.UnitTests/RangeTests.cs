#if NET9_0_OR_LOWER
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#endif
using System.Xml.Serialization;
using AwesomeAssertions.Execution;

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
        public void Empty_IsEqualToAnotherRangeThatIsEmpty_ButDoesNotHaveDefaultValues()
        {
            // Arrange
            var empty = Range<int>.Empty;
            var range = new Range<int>(10, 10);

            // Act - Assert
            empty.Should().Be(range);
        }

        [Theory]
        [MemberData(nameof(IsNullOrEmptyRangeData))]
        public void IsNullOrEmpty_ReturnsTheExpectedResult(Range<int> range, bool expectedResult)
        {
            // Act - Assert
            Range<int>.IsNullOrEmpty(range).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> IsNullOrEmptyRangeData()
        {
            // True
            yield return new object[] {new Range<int>(1, 99), false};

            // False
            yield return new object[] {Range<int>.Empty, true};
            yield return new object[] {null, true};
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
        public void Ctor_End_CannotBeNull()
        {
            // Arrange - Act
            // ReSharper disable once ObjectCreationAsStatement
            Action act = () => new Range<string>("", null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("end");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
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

        [Fact(Skip = "A solution for this problem has not been found yet")]
        public void Ctor_StartEnd_WhenTheStartOrEndReferenceChangesTheyAreNotChangedInsideTheRange()
        {
            // Arrange
            var start = new MyComparable {Value = 1};
            var end = new MyComparable {Value = 2};

            var range = new Range<MyComparable>(start, end);

            // Act
            start.Value = 3;

            // - Assert
            range.Start.Should().Be(new MyComparable {Value = 1});
            range.End.Should().Be(new MyComparable {Value = 2});
        }

        private sealed class MyComparable : IComparable, IComparable<MyComparable>
        {
            public int Value { get; set; }

            public int CompareTo(MyComparable other)
            {
                return Value.CompareTo(other.Value);
            }

            public int CompareTo(object obj)
            {
                return CompareTo(obj as MyComparable);
            }
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "On purpose")]
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
        public void Includes_ForRange_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            bool expectedResult)
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
            yield return new object[] {new Range<int>(1, 99), Range<int>.Empty, false};
            yield return new object[] {new Range<int>(0, 99), Range<int>.Empty, false};
            yield return new object[] {new Range<int>(-10, 10), Range<int>.Empty, false};
            yield return new object[] {new Range<int>(0, 99), null, false};
            yield return new object[] {Range<int>.Empty, new Range<int>(0, 99), false};
        }

        [Fact]
        public void IncludesAll_ReturnsTrue_GivenAllValuesArePartOfTheRange()
        {
            // Arrange 
            var range = new Range<int>(0, 10);

            // Act - Assert
            range.IncludesAll(0, 1, 2, 3, 4, 5, 6, 7, 8, 9).Should().BeTrue();
            range.IncludesAll(new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}).Should().BeTrue();
        }

        [Fact]
        public void IncludesAll_ReturnsFalse_GivenOneOfTheValuesAreNotPartOfTheRange()
        {
            // Arrange 
            var range = new Range<int>(0, 10);

            // Act - Assert
            range.IncludesAll(0, 1, 2, 3, 4, 99, 6, 7, 8, 9).Should().BeFalse();
            range.IncludesAll(new List<int> {0, 1, 2, 3, 4, 99, 6, 7, 8, 9}).Should().BeFalse();
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
        public void IsEmpty_ReturnsTrue_WhenRangeEmpty_ValueType()
        {
            // Arrange 
            var range = Range<int>.Empty;

            // Act - Assert
            range.IsEmpty().Should().BeTrue();
        }

        [Fact]
        public void IsEmpty_ReturnsTrue_WhenRangeEmpty_ReferenceType()
        {
            // Arrange 
            var range = Range<Version>.Empty;

            // Act - Assert
            range.IsEmpty().Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(OverlapsRangeData))]
        public void Overlaps_ForRange_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            range.Overlaps(otherRange).Should().Be(expectedResult);
            if (otherRange != null)
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
            yield return new object[] {new Range<int>(0, 10), Range<int>.Empty, false};
            yield return new object[] {new Range<int>(-10, 10), Range<int>.Empty, false};
            yield return new object[] {new Range<int>(0, 10), null, false};
        }

        [Theory]
        [MemberData(nameof(TouchesRangeData))]
        public void Touches_ForRange_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, bool expectedResult)
        {
            // Act - Assert
            range.Touches(otherRange).Should().Be(expectedResult);
            if (otherRange != null)
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

            // Do not touch because of null or Empty
            yield return new object[] {new Range<int>(0, 10), Range<int>.Empty, false};
            yield return new object[] {new Range<int>(0, 10), null, false};

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
            if (otherRange != null)
                otherRange.Gap(range).Should().Be(expectedGap);
        }

        public static IEnumerable<object[]> GapRangeData()
        {
            // Both are the same and thus no gap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(0, 10), Range<int>.Empty};

            // Both overlap and thus no gap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(5, 15), Range<int>.Empty};

            // Both touch each other, no gap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(10, 20), Range<int>.Empty};

            // Null or Empty
            yield return new object[] {new Range<int>(0, 10), Range<int>.Empty, Range<int>.Empty};
            yield return new object[] {new Range<int>(10, 20), Range<int>.Empty, Range<int>.Empty};
            yield return new object[] {new Range<int>(0, 10), null, Range<int>.Empty};

            // Gap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(20, 30), new Range<int>(10, 20)};
        }

        [Fact]
        public void Merge_MustOverlapOrTouch()
        {
            // Arrange
            var range = new Range<int>(0, 10);
            var otherRange = new Range<int>(15, 20);

            // Act
            Action act = () => range.Merge(otherRange);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith(
                    "Merging Range(0, 10) with Range(15, 20) is not possible because they do not overlap nor touch each other");
        }

        [Fact]
        public void Merge_WithEmpty_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);
            var otherRange = Range<int>.Empty;

            // Act
            Action act1 = () => range.Merge(otherRange);
            Action act2 = () => otherRange.Merge(range);

            // Assert
            act1.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith(
                    "Merging Range(0, 10) with Range.Empty is not possible because they do not overlap nor touch each other");
            act2.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith(
                    "Merging Range.Empty with Range(0, 10) is not possible because they do not overlap nor touch each other");
        }

        [Fact]
        public void Merge_WithNull_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);

            // Act
            Action act = () => range.Merge(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("range");
        }

        [Theory]
        [MemberData(nameof(MergeRangeData))]
        public void Merge_ReturnsARange_ThatHasTheLowestStartAndHighestEndOfBothRanges(Range<int> range,
            Range<int> otherRange, Range<int> expectedMerge)
        {
            // Act - Assert
            range.Merge(otherRange).Should().Be(expectedMerge);
            otherRange.Merge(range).Should().Be(expectedMerge);
        }

        public static IEnumerable<object[]> MergeRangeData()
        {
            // Both are the same, the merged is the same
            yield return new object[] {new Range<int>(0, 10), new Range<int>(0, 10), new Range<int>(0, 10)};

            // Both overlap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(5, 15), new Range<int>(0, 15)};

            // Both touch each other
            yield return new object[] {new Range<int>(0, 10), new Range<int>(10, 20), new Range<int>(0, 20)};
        }

        [Fact]
        public void Split_CannotBePerformedOnAnEmptyRange()
        {
            // Arrange
            var range = Range<int>.Empty;

            // Act
            Action act = () => range.Split(20);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith("Splitting is not possible because the 20 is not included in Range.Empty");
        }

        [Fact]
        public void Split_ValueMustBeIncludedInTheRange()
        {
            // Arrange
            var range = new Range<int>(0, 10);

            // Act
            Action act = () => range.Split(20);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith("Splitting is not possible because the 20 is not included in Range(0, 10)");
        }

        [Theory]
        [MemberData(nameof(SplitRangeData))]
        public void Split_ReturnsATupleOfRanges_ThatAreSplitOnTheValue(Range<int> range, int value,
            (Range<int>, Range<int>) expectedSplit)
        {
            // Act - Assert
            range.Split(value).Should().Be(expectedSplit);
        }

        public static IEnumerable<object[]> SplitRangeData()
        {
            // Split in the middle
            yield return new object[] {new Range<int>(0, 10), 5, (new Range<int>(0, 5), new Range<int>(5, 10))};
            yield return new object[] {new Range<int>(0, 10), 2, (new Range<int>(0, 2), new Range<int>(2, 10))};
            yield return new object[] {new Range<int>(0, 10), 7, (new Range<int>(0, 7), new Range<int>(7, 10))};

            // Split at the start
            yield return new object[] {new Range<int>(0, 10), 0, (Range<int>.Empty, new Range<int>(0, 10))};

            // Split at the end
            yield return new object[] {new Range<int>(0, 10), 10, (new Range<int>(0, 10), Range<int>.Empty)};
        }

        [Fact]
        public void Intersection_MustOverlap()
        {
            // Arrange
            var range = new Range<int>(0, 10);
            var otherRange = new Range<int>(10, 20);

            // Act
            Action act = () => range.Intersection(otherRange);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith(
                    "Intersecting Range(0, 10) with Range(10, 20) is not possible because they do not overlap each other");
        }

        [Fact]
        public void Intersection_WithEmpty_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);
            var otherRange = Range<int>.Empty;

            // Act
            Action act1 = () => range.Intersection(otherRange);
            Action act2 = () => otherRange.Intersection(range);

            // Assert
            act1.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith(
                    "Intersecting Range(0, 10) with Range.Empty is not possible because they do not overlap each other");
            act2.Should().Throw<ArgumentException>()
                .And.Message.Should()
                .StartWith(
                    "Intersecting Range.Empty with Range(0, 10) is not possible because they do not overlap each other");
        }

        [Fact]
        public void Intersection_WithNull_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);

            // Act
            Action act = () => range.Intersection(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("range");
        }

        [Theory]
        [MemberData(nameof(IntersectionRangeData))]
        public void Intersection_ReturnsARange_ThatHasTheLowestStartAndHighestEndOfBothRanges(Range<int> range,
            Range<int> otherRange, Range<int> expectedIntersection)
        {
            // Act - Assert
            range.Intersection(otherRange).Should().Be(expectedIntersection);
            otherRange.Intersection(range).Should().Be(expectedIntersection);
        }

        public static IEnumerable<object[]> IntersectionRangeData()
        {
            // Both are the same, the intersection is the same
            yield return new object[] {new Range<int>(0, 10), new Range<int>(0, 10), new Range<int>(0, 10)};

            // Both overlap
            yield return new object[] {new Range<int>(0, 10), new Range<int>(5, 15), new Range<int>(5, 10)};
        }

        [Fact]
        public void Exclusive_IsNotPossibleOnEqualRanges()
        {
            // Arrange
            var range = new Range<int>(0, 10);
            var otherRange = new Range<int>(0, 10);

            // Act
            Action act = () => range.Exclusive(otherRange);

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("There are no Exclusive ranges when both are equal.");
        }

        [Fact]
        public void Exclusive_WithNull_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);

            // Act
            Action act = () => range.Exclusive(null);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("range");
        }

        [Theory]
        [MemberData(nameof(ExclusiveRangeData))]
        public void Exclusive_ReturnsATupleOfRanges_ThatRepresentTheNonOverlappingParts(Range<int> range,
            Range<int> otherRange, (Range<int>, Range<int>) expectedExclusive)
        {
            // Act - Assert
            range.Exclusive(otherRange).Should().Be(expectedExclusive);
        }

        public static IEnumerable<object[]> ExclusiveRangeData()
        {
            // Includes (subset)
            yield return new object[]
                {new Range<int>(0, 10), new Range<int>(0, 5), (Range<int>.Empty, new Range<int>(5, 10))};
            yield return new object[]
                {new Range<int>(0, 10), new Range<int>(5, 10), (new Range<int>(0, 5), Range<int>.Empty)};
            yield return new object[]
                {new Range<int>(0, 10), new Range<int>(2, 7), (new Range<int>(0, 2), new Range<int>(7, 10))};

            // Includes (superset)
            yield return new object[]
                {new Range<int>(0, 5), new Range<int>(0, 10), (Range<int>.Empty, new Range<int>(5, 10))};
            yield return new object[]
                {new Range<int>(5, 10), new Range<int>(0, 10), (new Range<int>(0, 5), Range<int>.Empty)};
            yield return new object[]
                {new Range<int>(2, 7), new Range<int>(0, 10), (new Range<int>(0, 2), new Range<int>(7, 10))};

            // Overlaps
            yield return new object[]
                {new Range<int>(0, 10), new Range<int>(5, 15), (new Range<int>(0, 5), new Range<int>(10, 15))};
            yield return new object[]
                {new Range<int>(5, 15), new Range<int>(0, 10), (new Range<int>(0, 5), new Range<int>(10, 15))};

            // Already exclusive
            yield return new object[]
                {new Range<int>(0, 10), new Range<int>(10, 20), (new Range<int>(0, 10), new Range<int>(10, 20))};
            yield return new object[]
                {new Range<int>(0, 5), new Range<int>(10, 15), (new Range<int>(0, 5), new Range<int>(10, 15))};

            //  Empty
            yield return new object[]
                {new Range<int>(0, 10), Range<int>.Empty, (new Range<int>(0, 10), Range<int>.Empty)};
            yield return new object[]
                {Range<int>.Empty, new Range<int>(0, 10), (Range<int>.Empty, new Range<int>(0, 10))};
            yield return new object[]
                {new Range<int>(0, 10), new Range<int>(10, 10), (new Range<int>(0, 10), Range<int>.Empty)};
            yield return new object[]
                {new Range<int>(10, 10), new Range<int>(0, 10), (Range<int>.Empty, new Range<int>(0, 10))};
        }

        [Fact]
        public void EnumerateBy_Step_CannotBeNull()
        {
            // Arrange
            var range = new Range<string>("a", "z");

            // Act
            Func<IEnumerable<string>> act = () => range.EnumerateBy(null, (value, step) => value + step);

            // Assert
            act.Enumerating().Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("step");
        }

        [Fact]
        public void EnumerateBy_Stepper_CannotBeNull()
        {
            // Arrange
            var range = new Range<string>("a", "z");

            // Act
            Func<IEnumerable<string>> act = () => range.EnumerateBy("c", null);

            // Assert
            act.Enumerating().Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("stepper");
        }

        [Theory]
        [MemberData(nameof(EnumerateByData))]
        public void EnumerateBy_ReturnsAnIEnumerableOfAllValuesBetweenStartAndEnd_GivenAStepAndStepperFunc_HavingTypeOfStepSameAsTypeOfRange(Range<int> range, int stepToUse, IEnumerable<int> expectedResult)
        {
            // Arranger - Act
            var enumerable = range.EnumerateBy(stepToUse, (value, step) => value + step);

            // Assert
            enumerable.Should().BeEquivalentTo(expectedResult, options => options.WithStrictOrdering());
        }

        public static IEnumerable<object[]> EnumerateByData()
        {
            // Standard example
            yield return new object[]
            {
                new Range<int>(15, 20),
                1, // Step
                new[] { 15, 16, 17, 18, 19 }
            };

            // Step different from 1
            yield return new object[]
            {
                new Range<int>(15, 20),
                3, // Step
                new[] { 15, 18 }
            };

            // Range as small as one step
            yield return new object[]
            {
                new Range<int>(15, 16),
                1, // Step
                new[] { 15 }
            };

            // Empty Range
            yield return new object[]
            {
                new Range<int>(15, 15),
                1, // Step
                Array.Empty<int>()
            };

            // Very big step
            yield return new object[]
            {
                new Range<int>(15, 20),
                100, // Step
                new[] { 15 }
            };
        }

        [Fact]
        public void EnumerateBy_ReturnsAnIEnumerableOfAllValuesBetweenStartAndEnd_GivenAStepAndStepperFunc_HavingTypeOfStepDifferFromTypeOfRange()
        {
            // Arrange
            var range = new Range<double>(15.0, 20.0);

            // Act
            var enumerable = range.EnumerateBy(1, (value, step) => value + step);

            // Assert
#pragma warning disable CA1861 // Avoid constant arrays as arguments
            enumerable.Should().BeEquivalentTo(new[] {15, 16, 17, 18, 19}, options => options.WithStrictOrdering());
#pragma warning restore CA1861 // Avoid constant arrays as arguments
        }

        [Fact]
        public void EnumerateBy_GivenAStepperThatResultsInAValueLowerThanStart_IsNotSupported()
        {
            // Arrange
            var range = new Range<int>(15, 20);

            // Act
            Func<IEnumerable<int>> act = () => range.EnumerateBy(1, (value, step) => value - step);

            // Assert
            act.Enumerating().Should().Throw<NotSupportedException>()
                .And.Message.Should().Be("Enumerating is not possible because the 14 is lower than the start of Range(15, 20).");
        }

        [Fact]
        public void EnumerateBy_GivenAStepperThatResultsInTheSameValueTwice_IsNotSupported()
        {
            // Arrange
            var range = new Range<int>(15, 20);

            // Act
            Func<IEnumerable<int>> act = () => range.EnumerateBy(1, (value, step) => value);

            // Assert
            act.Enumerating().Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The stepper should create a higher value on every step. It has returned 15 for two times in a row.");
        }

        [Fact]
        public void EnumerateBy_ReturnsAnEmptyList_WhenExecutedOnAnEmptyRange()
        {
            // Arrange
            var range = Range<int>.Empty;

            // Act
            var enumerable = range.EnumerateBy(1, (value, step) => value + step);

            // Assert
            enumerable.Should().BeEquivalentTo(Array.Empty<int>(), options => options.WithStrictOrdering());
        }

        [Fact]
        public void EnumerateBy_ReturnsAnEmptyList_WhenExecutedOnAnRangeWhereStartEqualsEnd()
        {
            // Arrange
            var range = new Range<int>(15, 15);

            // Act
            var enumerable = range.EnumerateBy(1, (value, step) => value + step);

            // Assert
            enumerable.Should().BeEquivalentTo(Array.Empty<int>(), options => options.WithStrictOrdering());
        }

        // TODO: EnumerateBy without stepper function (via dynamic or Expressions)

        [Fact]
        public void EnumerateBy_ReturnsAnIEnumerableOfAllValuesBetweenStartAndEnd_ForDatesAndTimespan()
        {
            // Arrange
            var startDate = new DateTimeOffset(2020, 8,1, 16, 45, 33, TimeSpan.FromHours(2));
            var endDate = new DateTimeOffset(2020, 8,2, 0, 0, 0, TimeSpan.FromHours(2));
            var range = new Range<DateTimeOffset>(startDate, endDate);

            // Act
            var enumerable = range.EnumerateBy(TimeSpan.FromMinutes(90), (value, step) => value.Add(step));

            // Assert
            enumerable.Should().BeEquivalentTo(new[]
            {
                new DateTimeOffset(2020,8,1, 16, 45, 33, TimeSpan.FromHours(2)),
                new DateTimeOffset(2020,8,1, 18, 15, 33, TimeSpan.FromHours(2)),
                new DateTimeOffset(2020,8,1, 19, 45, 33, TimeSpan.FromHours(2)),
                new DateTimeOffset(2020,8,1, 21, 15, 33, TimeSpan.FromHours(2)),
                new DateTimeOffset(2020,8,1, 22, 45, 33, TimeSpan.FromHours(2)),
            }, options => options.WithStrictOrdering());
        }

        [Fact]
        public void EnumerateBy_GivenAStepperThatReturnsARandomResult_IsNotSupported()
        {
            // Arrange
            var random = new Random();
            var range = new Range<int>(-200, 2000);

            using (new AssertionScope())
            {
                for (var s = 1; s <= 100; s++) // Poor man's XUnit Repeat
                {
                    // Act
                    Func<IEnumerable<int>> act = () => range.EnumerateBy(s, (value, step) => value + random.Next(-step, step));

                    // Assert
                    act.Enumerating().Should().Throw<Exception>();
                }
            }
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "On purpose")]
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

        private sealed class NumericRange : Range<int>
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
        public void Equals_IsTrue_WhenBothAreEmpty()
        {
            // Arrange
            var range = new Range<int>(99, 99);
            var otherRange = new Range<int>(10, 10);

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
        public void CompareTo_IsZero_WhenRangesAreBothEmpty()
        {
            // Arrange
            var range = new Range<int>(10, 10);
            var otherRange = Range<int>.Empty;

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

        [Theory]
        [MemberData(nameof(CompareToWhenOtherIsEmptyData))]
        public void CompareTo_IsOne_WhenOtherIsEmpty(Range<int> range, Range<int> otherRange)
        {
            // Act - Assert
            otherRange.Should().Be(Range<int>.Empty);

            range.CompareTo(otherRange).Should().Be(1);
            range.CompareTo((object) otherRange).Should().Be(1);
        }

        public static IEnumerable<object[]> CompareToWhenOtherIsEmptyData()
        {
            yield return new object[] {new Range<int>(1, 99), Range<int>.Empty};
            yield return new object[] {new Range<int>(-99, -1), Range<int>.Empty};

            yield return new object[] {new Range<int>(1, 99), new Range<int>(99, 99)};
            yield return new object[] {new Range<int>(-99, -1), new Range<int>(-99, -99)};
        }

        [Theory]
        [MemberData(nameof(CompareToWhenCurrentIsEmptyData))]
        public void CompareTo_IsMinusOne_WhenCurrentIsEmpty(Range<int> range, Range<int> otherRange)
        {
            // Act - Assert
            range.Should().Be(Range<int>.Empty);

            range.CompareTo(otherRange).Should().Be(-1);
            range.CompareTo((object) otherRange).Should().Be(-1);
        }

        public static IEnumerable<object[]> CompareToWhenCurrentIsEmptyData()
        {
            yield return new object[] {Range<int>.Empty, new Range<int>(1, 99)};
            yield return new object[] {Range<int>.Empty, new Range<int>(-99, -1)};

            yield return new object[] {new Range<int>(99, 99), new Range<int>(1, 99)};
            yield return new object[] {new Range<int>(-99, -99), new Range<int>(-99, -1)};
        }

        [Theory]
        [MemberData(nameof(IsCompletelyBeforeData))]
        public void IsCompletelyBefore_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            bool expectedResult)
        {
            // Act - Assert
            range.IsCompletelyBefore(otherRange).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> IsCompletelyBeforeData()
        {
            // Completely Before
            yield return new object[] {new Range<int>(1, 5), new Range<int>(10, 20), true};

            // Touching
            yield return new object[] {new Range<int>(1, 10), new Range<int>(10, 20), false};
            yield return new object[] {new Range<int>(20, 30), new Range<int>(10, 20), false};

            // Overlapping
            yield return new object[] {new Range<int>(1, 15), new Range<int>(10, 20), false};
            yield return new object[] {new Range<int>(15, 25), new Range<int>(10, 20), false};

            // Completely Behind
            yield return new object[] {new Range<int>(25, 30), new Range<int>(10, 20), false};

            // Empty / null
            yield return new object[] {new Range<int>(0, 10), null, false};
            yield return new object[] {new Range<int>(0, 10), Range<int>.Empty, false};
            yield return new object[] {Range<int>.Empty, Range<int>.Empty, false};
            yield return new object[] {Range<int>.Empty, new Range<int>(0, 10), true};
        }

        [Theory]
        [MemberData(nameof(IsCompletelyBehindData))]
        public void IsCompletelyBehind_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            bool expectedResult)
        {
            // Act - Assert
            range.IsCompletelyBehind(otherRange).Should().Be(expectedResult);
        }

        public static IEnumerable<object[]> IsCompletelyBehindData()
        {
            // Completely Behind
            yield return new object[] {new Range<int>(25, 30), new Range<int>(10, 20), true};

            // Touching
            yield return new object[] {new Range<int>(20, 30), new Range<int>(10, 20), false};
            yield return new object[] {new Range<int>(1, 10), new Range<int>(10, 20), false};

            // Overlapping
            yield return new object[] {new Range<int>(15, 25), new Range<int>(10, 20), false};
            yield return new object[] {new Range<int>(1, 15), new Range<int>(10, 20), false};

            // Completely Before
            yield return new object[] {new Range<int>(1, 5), new Range<int>(10, 20), false};

            // Empty
            yield return new object[] {new Range<int>(0, 10), null, true};
            yield return new object[] {new Range<int>(0, 10), Range<int>.Empty, true};
            yield return new object[] {Range<int>.Empty, Range<int>.Empty, false};
            yield return new object[] {Range<int>.Empty, new Range<int>(0, 10), false};
        }

        [Fact]
        public void AsTuple_ReturnsTheRangeAsValueTuple()
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
        public void GetHashCode_OnEmptyRange_DoesNotThrowAnException()
        {
            // Arrange
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => Range<string>.Empty.GetHashCode();

            // Act - Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Clone_CreatesANewRange_ThatIsEquallyTheSame()
        {
            // Arrange
            var range = new Range<int>(1, 99);

            // Act
            var clone = range.Clone();

            // Assert
            clone.Should().BeOfType<Range<int>>();
            clone.Should().NotBeSameAs(range);
            clone.Should().BeEquivalentTo(range);
        }

        [Fact]
        public void Clone_CreatesANewRange_ThatIsEquallyTheSame_ButAlsoClonesStartAndEndIfTheyImplementICloneable()
        {
            // Arrange
            var versionStart = new Version(1, 0);
            var versionEnd = new Version(2, 5);
            var range = new Range<Version>(versionStart, versionEnd);

            // Act
            var clone = range.Clone() as Range<Version>;

            // Assert
            clone.Should().NotBeNull();
            clone.Should().NotBeSameAs(range);
            clone.Should().BeEquivalentTo(range);

            clone.Start.Should().NotBeSameAs(range.Start);
            clone.Start.Should().BeEquivalentTo(range.Start);
            clone.End.Should().NotBeSameAs(range.End);
            clone.End.Should().BeEquivalentTo(range.End);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "On purpose")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "On purpose")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "On purpose")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "On purpose")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "On purpose")]
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "On purpose")]
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
        public void GreaterThanOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            bool expectedResult)
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

            // Null
            yield return new object[] {new Range<int>(1, 99), null, true};
            yield return new object[] {null, new Range<int>(1, 99), false};

            // Empty
            yield return new object[] {new Range<int>(1, 99), Range<int>.Empty, true};
            yield return new object[] {Range<int>.Empty, new Range<int>(1, 99), false};
            yield return new object[] {new Range<int>(-99, -1), Range<int>.Empty, true};
            yield return new object[] {Range<int>.Empty, new Range<int>(-99, -1), false};
        }

        [Theory]
        [MemberData(nameof(LessThanOperatorData))]
        public void LessThanOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            bool expectedResult)
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

            // Null
            yield return new object[] {new Range<int>(1, 99), null, false};
            yield return new object[] {null, new Range<int>(1, 99), true};

            // Empty
            yield return new object[] {new Range<int>(1, 99), Range<int>.Empty, false};
            yield return new object[] {Range<int>.Empty, new Range<int>(1, 99), true};
            yield return new object[] {new Range<int>(-99, -1), Range<int>.Empty, false};
            yield return new object[] {Range<int>.Empty, new Range<int>(-99, -1), true};
        }

        [Theory]
        [MemberData(nameof(GreaterThanOrEqualOperatorData))]
        public void GreaterThanOrEqualOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            bool expectedResult)
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

            // Null
            yield return new object[] {new Range<int>(1, 99), null, true};
            yield return new object[] {null, new Range<int>(1, 99), false};

            // Empty
            yield return new object[] {Range<int>.Empty, Range<int>.Empty, true};
            yield return new object[] {new Range<int>(-99, -99), new Range<int>(99, 99), true};

            yield return new object[] {new Range<int>(1, 99), Range<int>.Empty, true};
            yield return new object[] {Range<int>.Empty, new Range<int>(1, 99), false};
            yield return new object[] {new Range<int>(-99, -1), Range<int>.Empty, true};
            yield return new object[] {Range<int>.Empty, new Range<int>(-99, -1), false};
        }

        [Theory]
        [MemberData(nameof(LessThanOrEqualOperatorData))]
        public void LessThanOrEqualOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            bool expectedResult)
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

            // Null
            yield return new object[] {new Range<int>(1, 99), null, false};
            yield return new object[] {null, new Range<int>(1, 99), true};

            // Empty
            yield return new object[] {Range<int>.Empty, Range<int>.Empty, true};
            yield return new object[] {new Range<int>(-99, -99), new Range<int>(99, 99), true};

            yield return new object[] {new Range<int>(1, 99), Range<int>.Empty, false};
            yield return new object[] {Range<int>.Empty, new Range<int>(1, 99), true};
            yield return new object[] {new Range<int>(-99, -1), Range<int>.Empty, false};
            yield return new object[] {Range<int>.Empty, new Range<int>(-99, -1), true};
        }

        [Theory]
        [MemberData(nameof(MergeRangeData))]
        public void OrOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange, Range<int> expectedOr)
        {
            // Act - Assert
            (range | otherRange).Should().Be(expectedOr);
            (otherRange | range).Should().Be(expectedOr);
        }

        [Fact]
        public void OrOperator_WithNull_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);

            // Act
            Func<object> act1 = () => range | null;
            Func<object> act2 = () => null | range;

            // Assert
            act1.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("rightRange");
            act2.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("leftRange");
        }

        [Theory]
        [MemberData(nameof(IntersectionRangeData))]
        public void AndOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            Range<int> expectedAnd)
        {
            // Act - Assert
            (range & otherRange).Should().Be(expectedAnd);
            (otherRange & range).Should().Be(expectedAnd);
        }

        [Fact]
        public void AndOperator_WithNull_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);

            // Act
            Func<object> act1 = () => range & null;
            Func<object> act2 = () => null & range;

            // Assert
            act1.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("rightRange");
            act2.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("leftRange");
        }

        [Theory]
        [MemberData(nameof(ExclusiveRangeData))]
        public void XorOperator_ReturnsTheExpectedResult(Range<int> range, Range<int> otherRange,
            ValueTuple<Range<int>, Range<int>> expectedXor)
        {
            // Act - Assert
            (range ^ otherRange).Should().Be(expectedXor);
            //(otherRange ^ range).Should().Be(expectedXor);
        }

        [Fact]
        public void XorOperator_WithNull_IsNotPossible()
        {
            // Arrange
            var range = new Range<int>(0, 10);

            // Act
            Func<object> act1 = () => range ^ null;
            Func<object> act2 = () => null ^ range;

            // Assert
            act1.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("rightRange");
            act2.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("leftRange");
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
            Func<Range<int>> act = () => (Range<int>) (99, 1);

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
        public void ImplicitConversionOperator_WithNull_IsNotPossible()
        {
            // Arrange - Act
            Func<(int, int)> act = () => (ValueTuple<int, int>) (Range<int>) null;

            // - Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("range");
        }

        [Fact]
        public void ToString_ReturnsAnInformalStringDescribingTheRange()
        {
            // Arrange
            var range = new Range<int>(1, 99);
            var emptyRange = Range<int>.Empty;

            // Act - Assert
            range.ToString().Should().Be("Range(1, 99)");
            emptyRange.ToString().Should().Be("Range.Empty");
        }

#if NET9_0_OR_LOWER
        [Theory]
        [MemberData(nameof(SerializeDeserializeRangeData))]
        public void Serialize_Deserialize_Binary_DoesNotChangeTheRange(object range, Type typeOfRange)
        {
            // Arrange
            var stream = new MemoryStream();
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
#pragma warning disable SYSLIB0011
            IFormatter formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

            // Act
            formatter.Serialize(stream, range);
            stream.Seek(0, SeekOrigin.Begin);
            var result = formatter.Deserialize(stream);

            // Assert
            result.Should().Be(range, $"{typeOfRange}");
        }
#endif

        [Theory(Skip = "Until there is a solution for the private setters on Start & End")]
        [MemberData(nameof(SerializeDeserializeRangeData))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA5369:Use XmlReader for 'XmlSerializer.Deserialize()'", Justification = "On purpose")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA3075:Insecure DTD processing in XML", Justification = "On purpose")]
        public void Serialize_Deserialize_Xml_DoesNotChangeTheRange(object range, Type typeOfRange)
        {
            // Arrange
            var rangeSerializer = new XmlSerializer(range.GetType());
            var stream = new MemoryStream();

            // Act

            //using var textWriter = new StringWriter();
            //rangeSerializer.Serialize(textWriter, range);
            //var result = textWriter.ToString();

            rangeSerializer.Serialize(stream, range);
            stream.Seek(0, SeekOrigin.Begin);
            var result = rangeSerializer.Deserialize(stream);

            // Assert
            result.Should().Be(range, $"{typeOfRange}");
        }

        public static IEnumerable<object[]> SerializeDeserializeRangeData()
        {
            yield return new object[] {new Range<int>(0, 99), typeof(Range<int>)};
            yield return new object[] {new Range<double>(-0.5, -0.1), typeof(Range<double>) };
            yield return new object[] {new Range<TimeSpan>(TimeSpan.FromDays(10), TimeSpan.FromDays(15)), typeof(Range<TimeSpan>) };
            yield return new object[] {new Range<Version>(new Version(1, 0), new Version(1, 1)), typeof(Range<Version>) };
        }
    }
}