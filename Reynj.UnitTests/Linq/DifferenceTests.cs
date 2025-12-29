using System;
using System.Collections.Generic;
using System.Linq;
using Reynj.Linq;

namespace Reynj.UnitTests.Linq
{
    public class DifferenceTests
    {
        [Fact]
        public void Difference_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = new Range<int>[] { };

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Difference(null).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("second");
        }

        [Fact]
        public void Difference_AsNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Difference(new Range<int>[] { }).ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("first");
        }

        [Theory]
        [MemberData(nameof(DifferenceData))]
        public void Difference_ReturnsTheExpectedResult(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second, IEnumerable<Range<int>> expectedDifferenceOf)
        {
            // Act
            var exclusiveOf = first.Exclusive(second);

            // Assert
            exclusiveOf.Should().BeEquivalentTo(expectedDifferenceOf);
        }

        //[Theory]
        //[MemberData(nameof(DifferenceData))]
        //public void Difference_ReturnsTheExpectedResult_OtherWayAround(IEnumerable<Range<int>> first, IEnumerable<Range<int>> second, IEnumerable<Range<int>> expectedDifferenceOf)
        //{
        //    // Act
        //    var exclusiveOf = second.Exclusive(first);

        //    // Assert
        //    exclusiveOf.Should().BeEquivalentTo(expectedDifferenceOf);
        //}

        public static IEnumerable<object[]> DifferenceData()
        {
            // Empty Lists
            yield return new object[]
            {
                new List<Range<int>>(),
                new List<Range<int>>(),
                new List<Range<int>>()
            };

            // A single Range that is the same
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>()
            };

            // An empty Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    Range<int>.Empty
                }),
                new List<Range<int>>(),
                new List<Range<int>>()
            };

            // An empty Range combined with a single Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    Range<int>.Empty,
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>()
            };

            // Two touching Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(10, 20)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20)
                }),
                new List<Range<int>>()
            };

            // Included in the other Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20),
                    new Range<int>(5, 15)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 20)
                }),
                new List<Range<int>>()
            };

            // Non-overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(20, 30)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(20, 30)
                })
            };

           //// Complex
           //yield return new object[]
           //{
           //     new List<Range<int>>(new[]
           //     {
           //         new Range<int>(0, 5),
           //         new Range<int>(3, 10),
           //         new Range<int>(10, 15),
           //         new Range<int>(18, 20)
           //     }),
           //     new List<Range<int>>(new[]
           //     {
           //         new Range<int>(1, 8),
           //         new Range<int>(12, 25)
           //     }),
           //     new List<Range<int>>(new[]
           //     {
           //         new Range<int>(1, 8),
           //         new Range<int>(12, 15),
           //         new Range<int>(18, 20)
           //     })
           //};

           // // More Complex
           // yield return new object[]
           // {
           //     new List<Range<int>>(new[]
           //     {
           //         new Range<int>(0, 5),
           //         new Range<int>(10, 15),
           //         new Range<int>(20, 25)
           //     }),
           //     new List<Range<int>>(new[]
           //     {
           //         new Range<int>(-5, -2),
           //         new Range<int>(2, 7),
           //         new Range<int>(12, 17),
           //         new Range<int>(22, 27),
           //         new Range<int>(32, 37)
           //     }),
           //     new List<Range<int>>(new[]
           //     {
           //         new Range<int>(2, 5),
           //         new Range<int>(12, 15),
           //         new Range<int>(22, 25)
           //     })
           // };
        }
    }
}