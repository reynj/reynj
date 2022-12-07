using FluentAssertions;
using Xunit;
using Reynj.Extensions;

namespace Reynj.UnitTests.Extensions
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void MinValue_ForATypeThatHasNoMinValueField_IsNotPossible()
        {
            // Arrange
            var type = typeof(object);

            // Act
            Action act = () => type.MinValue<object>();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The type Object does not contain a field with name MinValue.");
        }

        [Fact]
        public void MinValue_ForAnObjectTypeThatReturnsNullAsMinValue_IsNotPossible()
        {
            // Arrange
            var type = typeof(CustomObjectType);

            // Act
            Action act = () => type.MinValue<CustomObjectType>();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .And.Message.Should()
                .Be("The value of the field MinValue on CustomObjectType is null and not allowed.");
        }

        [Theory]
        [MemberData(nameof(MinValueData))]
        public void MinValue_ReturnsTheExpectedMinValue<T>(Type type, object expectedMinValue)
        {
            // Arrange
            // Act
            var minValue = type.MinValue<T>();

            // Assert
            minValue.Should().Be(expectedMinValue);
        }

        public static IEnumerable<object[]> MinValueData()
        {
            // Built-in types
            yield return new object[] {typeof(byte), byte.MinValue};
            yield return new object[] {typeof(sbyte), sbyte.MinValue};
            yield return new object[] {typeof(char), char.MinValue};
            yield return new object[] {typeof(decimal), decimal.MinValue};
            yield return new object[] {typeof(double), double.MinValue};
            yield return new object[] {typeof(float), float.MinValue};
            yield return new object[] {typeof(int), int.MinValue};
            yield return new object[] {typeof(uint), uint.MinValue};
            yield return new object[] {typeof(long), long.MinValue};
            yield return new object[] {typeof(ulong), ulong.MinValue};
            yield return new object[] {typeof(short), short.MinValue};
            yield return new object[] {typeof(ushort), ushort.MinValue};

            // IComparable types
            yield return new object[] {typeof(DateTime), DateTime.MinValue};
            yield return new object[] {typeof(DateTimeOffset), DateTimeOffset.MinValue};
            yield return new object[] {typeof(TimeSpan), TimeSpan.MinValue};

            yield return new object[] {typeof(System.Data.SqlTypes.SqlByte), System.Data.SqlTypes.SqlByte.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlDateTime), System.Data.SqlTypes.SqlDateTime.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlDecimal), System.Data.SqlTypes.SqlDecimal.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlDouble), System.Data.SqlTypes.SqlDouble.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlInt16), System.Data.SqlTypes.SqlInt16.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlInt32), System.Data.SqlTypes.SqlInt32.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlInt64), System.Data.SqlTypes.SqlInt64.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlMoney), System.Data.SqlTypes.SqlMoney.MinValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlSingle), System.Data.SqlTypes.SqlSingle.MinValue};
        }

        [Fact]
        public void MaxValue_ForATypeThatHasNoMaxValueField_IsNotPossible()
        {
            // Arrange
            var type = typeof(object);

            // Act
            Action act = () => type.MaxValue<object>();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .And.Message.Should().Be("The type Object does not contain a field with name MaxValue.");
        }

        [Fact]
        public void MaxValue_ForAnObjectTypeThatReturnsNullAsMaxValue_IsNotPossible()
        {
            // Arrange
            var type = typeof(CustomObjectType);

            // Act
            Action act = () => type.MaxValue<CustomObjectType>();

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .And.Message.Should()
                .Be("The value of the field MaxValue on CustomObjectType is null and not allowed.");
        }

        [Theory]
        [MemberData(nameof(MaxValueData))]
        public void MaxValue_ReturnsTheExpectedMinValue<T>(Type type, object expectedMaxValue)
        {
            // Arrange
            // Act
            var minValue = type.MaxValue<T>();

            // Assert
            minValue.Should().Be(expectedMaxValue);
        }

        public static IEnumerable<object[]> MaxValueData()
        {
            // Built-in types
            yield return new object[] {typeof(byte), byte.MaxValue};
            yield return new object[] {typeof(sbyte), sbyte.MaxValue};
            yield return new object[] {typeof(char), char.MaxValue};
            yield return new object[] {typeof(decimal), decimal.MaxValue};
            yield return new object[] {typeof(double), double.MaxValue};
            yield return new object[] {typeof(float), float.MaxValue};
            yield return new object[] {typeof(int), int.MaxValue};
            yield return new object[] {typeof(uint), uint.MaxValue};
            yield return new object[] {typeof(long), long.MaxValue};
            yield return new object[] {typeof(ulong), ulong.MaxValue};
            yield return new object[] {typeof(short), short.MaxValue};
            yield return new object[] {typeof(ushort), ushort.MaxValue};

            // IComparable types
            yield return new object[] {typeof(DateTime), DateTime.MaxValue};
            yield return new object[] {typeof(DateTimeOffset), DateTimeOffset.MaxValue};
            yield return new object[] {typeof(TimeSpan), TimeSpan.MaxValue};

            yield return new object[] {typeof(System.Data.SqlTypes.SqlByte), System.Data.SqlTypes.SqlByte.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlDateTime), System.Data.SqlTypes.SqlDateTime.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlDecimal), System.Data.SqlTypes.SqlDecimal.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlDouble), System.Data.SqlTypes.SqlDouble.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlInt16), System.Data.SqlTypes.SqlInt16.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlInt32), System.Data.SqlTypes.SqlInt32.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlInt64), System.Data.SqlTypes.SqlInt64.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlMoney), System.Data.SqlTypes.SqlMoney.MaxValue};
            yield return new object[] {typeof(System.Data.SqlTypes.SqlSingle), System.Data.SqlTypes.SqlSingle.MaxValue};
        }

        public class CustomObjectType
        {
#pragma warning disable 414
            public static readonly CustomObjectType MinValue;
            public static readonly CustomObjectType MaxValue;
#pragma warning restore 414
        }
    }
}