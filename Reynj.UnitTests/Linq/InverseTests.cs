using Reynj.Linq;

namespace Reynj.UnitTests.Linq
{
    public class InverseTests
    {
        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "Specific unit test")]
        public void Inverse_WithNull_IsNotPossible()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Inverse().ToList();

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .And.ParamName.Should().Be("source");
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "Specific unit test")]
        public void Inverse_MinValueMustBeLessThanOrEqualToMaxValue()
        {
            // Arrange
            IEnumerable<Range<int>> ranges = new List<Range<int>>();

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action act = () => ranges.Inverse(10, -10).ToList();

            // Assert
            act.Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("maxValue must be greater than or equal to minValue");
        }

        [Theory]
        [MemberData(nameof(InverseValueTypeData))]
        public void Inverse_ValueType_ReturnsTheExpectedResult(IEnumerable<Range<int>> ranges, IEnumerable<Range<int>> expectedInversed)
        {
            // Act
            var reduced = ranges.Inverse();

            // Assert
            reduced.Should().BeEquivalentTo(expectedInversed);
        }

        [Theory]
        [MemberData(nameof(InverseValueTypeData))]
        public void Inverse_ValueType_ReturnsTheExpectedResult_AlsoForReversedLists(IEnumerable<Range<int>> ranges, IEnumerable<Range<int>> expectedInversed)
        {
            // Act
            var reduced = ranges.Inverse();

            // Assert
            reduced.Should().BeEquivalentTo(expectedInversed);
        }

        public static IEnumerable<object[]> InverseValueTypeData()
        {
            // Empty List
            yield return new object[]
            {
                new List<Range<int>>(),
                new List<Range<int>>
                {
                    new Range<int>(int.MinValue, int.MaxValue)
                }
            };

            // A single Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(10, int.MaxValue)
                })
            };

            // Range that goes from MinValue to MaxValue
            yield return new object[]
            {
                new List<Range<int>>
                {
                    new Range<int>(int.MinValue, int.MaxValue)
                },
                new List<Range<int>>()
            };

            // An empty Range
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    Range<int>.Empty
                }),
                new List<Range<int>>
                {
                    new Range<int>(int.MinValue, int.MaxValue)
                }
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(10, int.MaxValue)
                })
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(20, int.MaxValue)
                })
            };

            // Two overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(5, 15)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(15, int.MaxValue)
                })
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
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(20, int.MaxValue)
                })
            };

            // Non-overlapping Ranges
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(0, 10),
                    new Range<int>(20, 30)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(10, 20),
                    new Range<int>(30, int.MaxValue)
                })
            };

            // Mixed case
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(2, 10),
                    new Range<int>(-5, 1),
                    new Range<int>(30, 30),
                    new Range<int>(5, 20)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(int.MinValue, -5),
                    new Range<int>(1, 2),
                    new Range<int>(20, int.MaxValue)
                })
            };

            // Complex
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(50, 55),
                    new Range<int>(17, 25),
                    new Range<int>(3, 7),
                    new Range<int>(0, 1),
                    new Range<int>(2, 6),
                    new Range<int>(4, 9),
                    new Range<int>(27, 32),
                    new Range<int>(1, 7),
                    Range<int>.Empty,
                    new Range<int>(25, 41)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(9, 17),
                    new Range<int>(41, 50),
                    new Range<int>(55, int.MaxValue)
                })
            };

            // Complex with a MinValue
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(50, 55),
                    new Range<int>(17, 25),
                    new Range<int>(3, 7),
                    new Range<int>(0, 1),
                    new Range<int>(2, 6),
                    new Range<int>(4, 9),
                    new Range<int>(27, 32),
                    new Range<int>(1, 7),
                    Range<int>.Empty,
                    new Range<int>(25, 41),
                    new Range<int>(int.MinValue, -10)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(-10, 0),
                    new Range<int>(9, 17),
                    new Range<int>(41, 50),
                    new Range<int>(55, int.MaxValue)
                })
            };

            // Complex with a MaxValue
            yield return new object[]
            {
                new List<Range<int>>(new[]
                {
                    new Range<int>(50, 55),
                    new Range<int>(17, 25),
                    new Range<int>(3, 7),
                    new Range<int>(0, 1),
                    new Range<int>(2, 6),
                    new Range<int>(4, 9),
                    new Range<int>(27, 32),
                    new Range<int>(1, 7),
                    Range<int>.Empty,
                    new Range<int>(25, 41),
                    new Range<int>(100, int.MaxValue)
                }),
                new List<Range<int>>(new[]
                {
                    new Range<int>(int.MinValue, 0),
                    new Range<int>(9, 17),
                    new Range<int>(41, 50),
                    new Range<int>(55, 100)
                })
            };
        }

        [Theory]
        [MemberData(nameof(InverseReferenceTypeData))]
        public void Inverse_ReferenceType_ReturnsTheExpectedResult(IEnumerable<Range<Version>> ranges, IEnumerable<Range<Version>> expectedInversed)
        {
            // Arrange
            var minValue = new Version(0, 0, 0, 0);
            var maxValue = new Version(9999, 9999, 9999, 99999);

            // Act
            var reduced = ranges.Inverse(minValue, maxValue);

            // Assert
            reduced.Should().BeEquivalentTo(expectedInversed);
        }

        [Theory]
        [MemberData(nameof(InverseReferenceTypeData))]
        public void Inverse_ReferenceType_ReturnsTheExpectedResult_AlsoForReversedLists(IEnumerable<Range<Version>> ranges, IEnumerable<Range<Version>> expectedInversed)
        {
            // Arrange
            var minValue = new Version(0, 0, 0, 0);
            var maxValue = new Version(9999, 9999, 9999, 99999);

            // Act
            var reduced = ranges.Inverse(minValue, maxValue);

            // Assert
            reduced.Should().BeEquivalentTo(expectedInversed);
        }

        public static IEnumerable<object[]> InverseReferenceTypeData()
        {
            var minValue = new Version(0, 0, 0, 0);
            var maxValue = new Version(9999, 9999, 9999, 99999);

            // Empty List
            yield return new object[]
            {
                new List<Range<Version>>(),
                new List<Range<Version>>
                {
                    new Range<Version>(minValue, maxValue)
                }
            };

            // A single Range
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("0.0.10.0"), new Version("10.0.0.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.10.0")),
                    new Range<Version>(new Version("10.0.0.0"), maxValue)
                })
            };

            // Range that goes from MinValue to MaxValue
            yield return new object[]
            {
                new List<Range<Version>>
                {
                    new Range<Version>(minValue, maxValue)
                },
                new List<Range<Version>>()
            };

            // An empty Range
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    Range<Version>.Empty
                }),
                new List<Range<Version>>
                {
                    new Range<Version>(minValue, maxValue)
                }
            };

            // An empty Range combined with a single Range
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    Range<Version>.Empty,
                    new Range<Version>(new Version("0.0.10.0"), new Version("10.0.0.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.10.0")),
                    new Range<Version>(new Version("10.0.0.0"), maxValue)
                })
            };

            // Two touching Ranges
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("0.0.10.0"), new Version("10.0.0.0")),
                    new Range<Version>(new Version("10.0.0.0"), new Version("10.2.0.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.10.0")),
                    new Range<Version>(new Version("10.2.0.0"), maxValue)
                })
            };

            // Two overlapping Ranges
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("0.0.10.0"), new Version("10.0.0.0")),
                    new Range<Version>(new Version("0.5.0.0"), new Version("10.5.0.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.10.0")),
                    new Range<Version>(new Version("10.5.0.0"), maxValue)
                })
            };

            // Included in the other Range
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("0.0.10.0"), new Version("10.20.0.0")),
                    new Range<Version>(new Version("0.5.0.0"), new Version("10.5.0.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.10.0")),
                    new Range<Version>(new Version("10.20.0.0"), maxValue)
                })
            };

            // Non-overlapping Ranges
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("0.0.10.0"), new Version("10.0.0.0")),
                    new Range<Version>(new Version("10.20.0.0"), new Version("30.3.3.3"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.10.0")),
                    new Range<Version>(new Version("10.0.0.0"), new Version("10.20.0.0")),
                    new Range<Version>(new Version("30.3.3.3"), maxValue)
                })
            };

            // Mixed case
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("0.2.0.0"), new Version("0.10.0.0")),
                    new Range<Version>(new Version("0.0.00.05"), new Version("0.0.1.01")),
                    new Range<Version>(new Version("0.0.30.0"), new Version("0.0.30.0")),
                    new Range<Version>(new Version("0.5.0.0"), new Version("0.20.0.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.00.05")),
                    new Range<Version>(new Version("0.0.1.01"), new Version("0.2.0.0")),
                    new Range<Version>(new Version("0.20.0.0"), maxValue)
                })
            };

            // Complex
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("50.0"), new Version("55.0")),
                    new Range<Version>(new Version("17.0"), new Version("25.0")),
                    new Range<Version>(new Version("3.0"), new Version("7.0")),
                    new Range<Version>(new Version("0.0.0.00001"), new Version("1.0")),
                    new Range<Version>(new Version("2.0"), new Version("6.0")),
                    new Range<Version>(new Version("4.0"), new Version("9.0")),
                    new Range<Version>(new Version("27.0"), new Version("32.0")),
                    new Range<Version>(new Version("1.0"), new Version("7.0")),
                    Range<Version>.Empty,
                    new Range<Version>(new Version("25.0"), new Version("41.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.0.00001")),
                    new Range<Version>(new Version("9.0"), new Version("17.0")),
                    new Range<Version>(new Version("41.0"), new Version("50.0")),
                    new Range<Version>(new Version("55.0"), maxValue)
                })
            };

            // Complex with a MinValue
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("50.0"), new Version("55.0")),
                    new Range<Version>(new Version("17.0"), new Version("25.0")),
                    new Range<Version>(new Version("3.0"), new Version("7.0")),
                    new Range<Version>(new Version("0.0.0.00001"), new Version("1.0")),
                    new Range<Version>(new Version("2.0"), new Version("6.0")),
                    new Range<Version>(new Version("4.0"), new Version("9.0")),
                    new Range<Version>(new Version("27.0"), new Version("32.0")),
                    new Range<Version>(new Version("1.0"), new Version("7.0")),
                    Range<Version>.Empty,
                    new Range<Version>(new Version("25.0"), new Version("41.0")),
                    new Range<Version>(minValue, new Version("2.0"))
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("9.0"), new Version("17.0")),
                    new Range<Version>(new Version("41.0"), new Version("50.0")),
                    new Range<Version>(new Version("55.0"), maxValue)
                })
            };

            // Complex with a MaxValue
            yield return new object[]
            {
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(new Version("50.0"), new Version("55.0")),
                    new Range<Version>(new Version("17.0"), new Version("25.0")),
                    new Range<Version>(new Version("3.0"), new Version("7.0")),
                    new Range<Version>(new Version("0.0.0.00001"), new Version("1.0")),
                    new Range<Version>(new Version("2.0"), new Version("6.0")),
                    new Range<Version>(new Version("4.0"), new Version("9.0")),
                    new Range<Version>(new Version("27.0"), new Version("32.0")),
                    new Range<Version>(new Version("1.0"), new Version("7.0")),
                    Range<Version>.Empty,
                    new Range<Version>(new Version("25.0"), new Version("41.0")),
                    new Range<Version>(new Version("100.0"), maxValue)
                }),
                new List<Range<Version>>(new[]
                {
                    new Range<Version>(minValue, new Version("0.0.0.00001")),
                    new Range<Version>(new Version("9.0"), new Version("17.0")),
                    new Range<Version>(new Version("41.0"), new Version("50.0")),
                    new Range<Version>(new Version("55.0"), new Version("100.0"))
                })
            };
        }
    }
}