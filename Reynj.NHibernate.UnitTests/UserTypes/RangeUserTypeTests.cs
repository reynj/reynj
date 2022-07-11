using System;
using System.Collections.Generic;
using FluentAssertions;
using NHibernate;
using NHibernate.Type;
using NHibernate.UserTypes;
using Reynj.NHibernate.UserTypes;
using Xunit;

namespace Reynj.NHibernate.UnitTests.UserTypes
{
    public class RangeUserTypeTests
    {
        [Fact]
        public void PropertyNames_AreStartAndEnd()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act - Assert
            rangeUserType.PropertyNames.Should().Equal("Start", "End");
        }

        [Fact]
        public void PropertyTypes_AreTheSame()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act - Assert
            rangeUserType.PropertyTypes.Should()
                .HaveCount(2)
                .And.AllBeEquivalentTo(NHibernateUtil.Int32);
        }

        [Theory]
        [MemberData(nameof(PropertyTypesData))]
        public void PropertyTypes_MatchWithTheGivenGenericType(Type dotnetType, IType nhibernateType)
        {
            // Arrange
            var rangeUserType = (ICompositeUserType) Activator.CreateInstance(typeof(RangeUserType<>)
                .MakeGenericType(dotnetType));

            // Act - Assert
            rangeUserType.PropertyTypes.Should().AllBeEquivalentTo(nhibernateType);
        }

        public static IEnumerable<object[]> PropertyTypesData()
        {
            // Built-in types
            yield return new object[] { typeof(byte), NHibernateUtil.Byte };
            yield return new object[] { typeof(sbyte), NHibernateUtil.SByte };
            yield return new object[] { typeof(char), NHibernateUtil.Character };
            yield return new object[] { typeof(string), NHibernateUtil.String };
            yield return new object[] { typeof(decimal), NHibernateUtil.Decimal };
            yield return new object[] { typeof(double), NHibernateUtil.Double };
            yield return new object[] { typeof(float), NHibernateUtil.Single };
            yield return new object[] { typeof(int), NHibernateUtil.Int32 };
            yield return new object[] { typeof(uint), NHibernateUtil.UInt32 };
            yield return new object[] { typeof(long), NHibernateUtil.Int64 };
            yield return new object[] { typeof(ulong), NHibernateUtil.UInt64 };
            yield return new object[] { typeof(short), NHibernateUtil.Int16 };
            yield return new object[] { typeof(ushort), NHibernateUtil.UInt16 };

            // IComparable types
            yield return new object[] { typeof(DateTime), NHibernateUtil.DateTime };
            yield return new object[] { typeof(DateTimeOffset), NHibernateUtil.DateTimeOffset };
            yield return new object[] { typeof(TimeSpan), NHibernateUtil.TimeSpan };
        }

        [Fact]
        public void GetPropertyValue_ReturnsTheStartOrTheEndValue_ForTheCorrespondingPropertyIndex()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act - Assert
            rangeUserType.GetPropertyValue(new Range<int>(-10, 10), 0).Should().Be(-10);
            rangeUserType.GetPropertyValue(new Range<int>(-10, 10), 1).Should().Be(10);
        }

        [Fact]
        public void GetPropertyValue_ForAPropertyIndexThatIsOutOfRange_ThrowsAnArgumentOutOfRangeException()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act
            Action act = () => rangeUserType.GetPropertyValue(new Range<int>(-10, 10), -5);

            // Assert
            var assertion = act.Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("The parameter property must have the value 0 or 1.*");

            assertion.And.ParamName.Should().Be("property");
            assertion.And.ActualValue.Should().Be(-5);
        }

        [Fact]
        public void GetPropertyValue_WithComponentNull_ThrowsAnArgumentNullException()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act
            Action act = () => rangeUserType.GetPropertyValue(null!, 0);

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("component");
        }

        [Fact]
        public void SetPropertyValue_IsNotSupportedForImmutableTypes()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act
            Action act = () => rangeUserType.SetPropertyValue(null!, 0, null!);

            // Assert
            act.Should().Throw<NotSupportedException>()
                .And.Message.Should().Be("SetPropertyValue is not supported on immutable types.");
        }

        [Theory]
        [MemberData(nameof(ReturnedClassData))]
        public void ReturnedClass_MatchWithTheGivenGenericType(Type dotnetType, Type rangeType)
        {
            // Arrange
            var rangeUserType = (ICompositeUserType) Activator.CreateInstance(typeof(RangeUserType<>)
                .MakeGenericType(dotnetType));

            // Act - Assert
            rangeUserType.ReturnedClass.Should().Be(rangeType);
        }

        public static IEnumerable<object[]> ReturnedClassData()
        {
            // Built-in types
            yield return new object[] { typeof(byte), typeof(Range<byte>) };
            yield return new object[] { typeof(sbyte), typeof(Range<sbyte>) };
            yield return new object[] { typeof(char), typeof(Range<char>) };
            yield return new object[] { typeof(string), typeof(Range<string>) };
            yield return new object[] { typeof(decimal), typeof(Range<decimal>) };
            yield return new object[] { typeof(double), typeof(Range<double>) };
            yield return new object[] { typeof(float), typeof(Range<float>) };
            yield return new object[] { typeof(int), typeof(Range<int>) };
            yield return new object[] { typeof(uint), typeof(Range<uint>) };
            yield return new object[] { typeof(long), typeof(Range<long>) };
            yield return new object[] { typeof(ulong), typeof(Range<ulong>) };
            yield return new object[] { typeof(short), typeof(Range<short>) };
            yield return new object[] { typeof(ushort), typeof(Range<ushort>) };

            // IComparable types
            yield return new object[] { typeof(DateTime), typeof(Range<DateTime>) };
            yield return new object[] { typeof(DateTimeOffset), typeof(Range<DateTimeOffset>) };
            yield return new object[] { typeof(TimeSpan), typeof(Range<TimeSpan>) };
        }

        [Theory]
        [MemberData(nameof(EqualsData))]
        public void Equals_DeterminesTheEqualityOfTwoObjects(object x, object y, bool expectedEquality)
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act - Assert
            rangeUserType.Equals(x, y).Should().Be(expectedEquality);
        }

        public static IEnumerable<object[]> EqualsData()
        {
            // Null
            yield return new object[] { null, null, true };
            yield return new object[] { null, new(), false };

            // Ranges
            yield return new object[] { new Range<int>(1, 10), new Range<int>(1, 10), true };
            yield return new object[] { new Range<int>(1, 10), new Range<int>(10, 20), false };
            yield return new object[] { new Range<int>(1, 10), new Range<string>("a", "z"), false };

            // Other
            yield return new object[] { "abc", "abc", true };
            yield return new object[] { "abc", "123", false };
            yield return new object[] { "abc", 123, false };
        }

        [Theory]
        [MemberData(nameof(GetHashCodeData))]
        public void GetHashCode_ReturnsTheHashCodeOfAnObject(object x, int expectedHashCode)
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act - Assert
            rangeUserType.GetHashCode(x).Should().Be(expectedHashCode);
        }

        public static IEnumerable<object[]> GetHashCodeData()
        {
            // Null
            yield return new object[] { null, 0 };

            // Ranges
            yield return new object[] { new Range<int>(1, 10), new Range<int>(1, 10).GetHashCode() };

            // Other
            yield return new object[] { "abc", "abc".GetHashCode() };
        }

        // TODO: NullSafeGet & NullSafeSet

        // TODO: IntegrationTest

        [Fact]
        public void DeepCopy_ReturnsACloneOfAGivenRange()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<int>(-10, 10);

            // Act
            var copy = rangeUserType.DeepCopy(range);

            // Assert
            copy.Should().BeOfType<Range<int>>();
            copy.Should().NotBeSameAs(range);
            copy.Should().BeEquivalentTo(range);
        }

        [Fact]
        public void DeepCopy_ReturnsNull_WhenGivenRangeIsOfADifferentTypeThanTheRangeUserType()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<string>("a", "z");

            // Act
            rangeUserType.DeepCopy(range).Should().BeNull();
        }

        [Fact]
        public void DeepCopy_ReturnsNull_WhenGivenObjectIsNull()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act
            rangeUserType.DeepCopy(null).Should().BeNull();
        }

        [Fact]
        public void IsMutable_ReturnsFalse()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act - Assert
            rangeUserType.IsMutable.Should().BeFalse();
        }

        [Fact]
        public void Disassemble_ReturnsACloneOfAGivenRange()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<int>(-10, 10);

            // Act
            var copy = rangeUserType.Disassemble(range, null!);

            // Assert
            copy.Should().BeOfType<Range<int>>();
            copy.Should().NotBeSameAs(range);
            copy.Should().BeEquivalentTo(range);
        }

        [Fact]
        public void Disassemble_ReturnsNull_WhenGivenRangeIsOfADifferentTypeThanTheRangeUserType()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<string>("a", "z");

            // Act
            rangeUserType.Disassemble(range, null!).Should().BeNull();
        }

        [Fact]
        public void Disassemble_ReturnsNull_WhenGivenObjectIsNull()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act
            rangeUserType.Disassemble(null, null!).Should().BeNull();
        }

        [Fact]
        public void Assemble_ReturnsACloneOfAGivenRange()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<int>(-10, 10);

            // Act
            var copy = rangeUserType.Assemble(range, null!, null);

            // Assert
            copy.Should().BeOfType<Range<int>>();
            copy.Should().NotBeSameAs(range);
            copy.Should().BeEquivalentTo(range);
        }

        [Fact]
        public void Assemble_ReturnsNull_WhenGivenRangeIsOfADifferentTypeThanTheRangeUserType()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<string>("a", "z");

            // Act
            rangeUserType.Assemble(range, null!, null).Should().BeNull();
        }

        [Fact]
        public void Assemble_ReturnsNull_WhenGivenObjectIsNull()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act
            rangeUserType.Assemble(null, null!, null).Should().BeNull();
        }

        [Fact]
        public void Replace_ReturnsACloneOfAGivenRange()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<int>(-10, 10);

            // Act
            var copy = rangeUserType.Replace(range, null, null!, null);

            // Assert
            copy.Should().BeOfType<Range<int>>();
            copy.Should().NotBeSameAs(range);
            copy.Should().BeEquivalentTo(range);
        }

        [Fact]
        public void Replace_ReturnsNull_WhenGivenRangeIsOfADifferentTypeThanTheRangeUserType()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();
            var range = new Range<string>("a", "z");

            // Act
            rangeUserType.Replace(range, null, null!, null).Should().BeNull();
        }

        [Fact]
        public void Replace_ReturnsNull_WhenGivenObjectIsNull()
        {
            // Arrange
            var rangeUserType = new RangeUserType<int>();

            // Act
            rangeUserType.Replace(null, null, null!, null).Should().BeNull();
        }
    }
}
