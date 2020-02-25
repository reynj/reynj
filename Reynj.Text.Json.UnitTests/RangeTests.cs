using System;
using System.Collections.Generic;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace Reynj.Text.Json.UnitTests
{
    public class RangeTests
    {
        [Theory]
        [MemberData(nameof(SerializeDeserializeRangeData))]
        public void Serialize_Deserialize_Json_DoesNotChangeTheRange(object range, Type typeOfRange)
        {
            // Arrange
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new RangeConverter()
                }
            };
            
            // Act
            var jsonRange = JsonSerializer.Serialize(range, options);
            var result = JsonSerializer.Deserialize(jsonRange, typeOfRange, options);

            // Assert
            result.Should().Be(range);
        }

        public static IEnumerable<object[]> SerializeDeserializeRangeData()
        {
            yield return new object[] { new Range<int>(0, 99), typeof(Range<int>) };
            yield return new object[] { new Range<double>(-0.5, -0.1), typeof(Range<double>) };
            //yield return new object[] { new Range<TimeSpan>(TimeSpan.FromDays(10), TimeSpan.FromDays(15)), typeof(Range<TimeSpan>) };
            //yield return new object[] { new Range<Version>(new Version(1, 0), new Version(1, 1)), typeof(Range<Version>) };
        }
    }
}
