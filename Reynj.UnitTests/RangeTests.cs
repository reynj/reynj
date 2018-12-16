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
    }
}
