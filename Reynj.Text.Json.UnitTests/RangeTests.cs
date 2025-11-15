using System.Text.Json;
using FluentAssertions.Execution;

namespace Reynj.Text.Json.UnitTests
{
    public class RangeTests
    {
        [Theory]
        [MemberData(nameof(SerializeDeserializeRangeData))]
        public void Serialize_Deserialize_Json_DoesNotChangeTheRange(object range, Type typeOfRange, string expectedJson)
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
            var json = JsonSerializer.Serialize(range, options);
            var result = JsonSerializer.Deserialize(json, typeOfRange, options);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(range);
                json.Should().Be(expectedJson);
            }
        }

        public static IEnumerable<object[]> SerializeDeserializeRangeData()
        {
            yield return new object[] { Range<int>.Empty, typeof(Range<int>), @"{}" };
            yield return new object[] { Range<string>.Empty, typeof(Range<string>), @"{}" };
            yield return new object[] { Range<Version>.Empty, typeof(Range<Version>), @"{}" };
            yield return new object[] { null, typeof(Range<int>), "null" };
            yield return new object[] { new Range<int>(0, 99), typeof(Range<int>), @"{""Start"":0,""End"":99}" };
            yield return new object[] { new Range<double>(-0.5, -0.1), typeof(Range<double>), @"{""Start"":-0.5,""End"":-0.1}" };
            //yield return new object[] { new Range<TimeSpan>(TimeSpan.FromDays(10), TimeSpan.FromDays(15)), typeof(Range<TimeSpan>), @"{""Start"":""10.00:00:00"",""End"":""15.00:00:00""}" };
            //yield return new object[] { new Range<Version>(new Version(1, 0), new Version(1, 1)), typeof(Range<Version>), @"{""Start"":""1.0"",""End"":""1.1""}" };
        }
    }
}
