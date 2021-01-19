# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
<!--
- The Difference method on IEnumerable<Range<T>> to get the relative complement of two sequences and also reduce the result
- The Exclusive method on IEnumerable<Range<T>> to get the symmetric difference of two sequences and also reduce the result
-->
### Changed
- ArgumentNullException is thrown when end of the Range is null, instead of an ArgumentException with message 'end must be greater than or equal to start' 

## [1.3.1] - 2020-11-12
### Added
- NuGet Packages with .NET 5.0 as TargetFramework

## [1.3.0] - 2020-08-08
### Added
- The IsSingle method on IEnumerable to check if the sequence contains exactly one element.
- EnumerateBy methods (protected) that can help in enumerating over all values within the Range<T>.

## [1.2.0] - 2020-03-01
### Added
- `Range<T>` implements `IClonable`
- Extension methods to convert between `Range<int>` and `System.Range` (only .NET Standard 2.1 and higher)
- `Serializable` attribute on `Range<T>` to support serialization and deserialization (at the moment only binary serialization is supported)
- Support for JSON serialization and deserialization with the `System.Text.Json` library (see [Reynj.Text.Json](https://www.nuget.org/packages/Reynj.Text.Json))
- Support for JSON serialization and deserialization with the `Newtonsoft.Json` library (see [Reynj.Newtonsoft.Json](https://www.nuget.org/packages/Reynj.Newtonsoft.Json))

## [1.1.0] - 2020-02-07
### Added
- The IsContiguous method on IEnumerable<Range<T>> to check if the sequence only contains touching Ranges and form a contiguous sequence.

### Fixed
- IsEmpty() now works with reference types

## [1.0.0] - 2019-01-30
### Added
- The Reduce method on IEnumerable<Range<T>>, returns a new IEnumerable<Range<T>> where all overlapping and touching Ranges have been merged and empty Ranges have been removed
- The Union method on IEnumerable<Range<T>> to union two sequences and also reduce the result
- The Intersect method on IEnumerable<Range<T>>, returns a new IEnumerable<Range<T>> where only Ranges that occur in both sequences are returned
- IsCompletelyBefore and IsCompletelyBehind methods on Range<T>, the return if a given Range<T> is lower than or greater than the specified range and that they do not touch nor overlap each other
- The Inverse method on IEnumerable<Range<T>>, returns a new IEnumerable<Range<T>> with all gaps between the given Ranges and also a range from the minvalue to the start of the first range and the end of the last range to the maxvalue

### Removed 
- RangeCollection<T> has been removed in favor of Linq extension methods on IEnumerable<Range<T>>

### Changed
- The Highest and Lowest methods have been moved to an extension method on IEnumerable<Range<T>>

## [0.2.0] - 2019-01-02
### Added
- The Exclusive method, that returns a Tuple of two Ranges that represent the parts they do not have in common, including the ^ operator
- A RangeCollection<T>, that holds a list of Ranges
- The Lowest and Highest methods on a RangeCollection<T> that return the lowest start and highest end of all the Ranges in the collection

### Changed
- The CompareTo method on how it handles null and Range<T>.Empty. Range<T>.Empty is always lower than any other Range, except when both are empty.

### Fixed
- All operators now handle null on each side of the operator correctly

## [0.1.5] - 2018-12-27
### Added
- A static IsNullOrEmpty method, to check if a Range<T> is null or Range<T>.Empty
- The Merge method, to create a single Range<T> from two overlapping or touching Ranges, including the | operator
- The Split method, to split the Range<T> at a given value into a Tuple of two Ranges
- The Intersection method, that returns the Range<T> that is part of both Ranges, including the & operator

### Changed
- The implementation of Equals and CompareTo so that every Empty Range, ranges with the same start and end, are equal regarding of the value of start and end
- The implementation of Includes, Overlaps, Touches and Gap in combination with the current Range<T> bieng Empty and/or the specified Range<T> bieng Empty

## [0.1.4] - 2018-12-23
### Added
- The Includes(T value) method was added to Range<T>, it returns true if the given value is a part of the Range
- The Includes(Range<T> range) method was added to Range<T>, it returns true if the given Range is completely a part of the Range
- The IncludesAll(params T value) method was added to Range<T>, it returns true if all of the given values are a part of the Range
- The IsEmpty method was added to Range<T>, it returns true if the start and end of the Range are equal
- Support for conversion to and from a Tuple/ValueTuple has been added
- Static property Empty that is a representation of an Empty Range
- The Overlaps to determine if two Ranges overlap
- The Touches to determine if two Ranges touch each other
- The Gap method that returns the difference or gap between two Ranges as a new Range<T>

## [0.1.3] - 2018-12-18
### Changed
- Build and Deployment changes
- Minor documentation changes

## 0.1.2 - 2018-12-18
### Added
- XML Documentation Comments on all methods and properties of `Range<T>`

## 0.1.1 - 2018-12-18
### Added
- Basic implementation of a Range class based on this [article](https://martinfowler.com/eaaDev/Range.html) from Martin Fowler.
  `Range<T>` is a class that implements `IEquatable<T>` and `IComparable<T>` including their operators. It has a single constructor that takes
  two `IComparable<T>` parameters, start and end, where start <= end.
- This CHANGELOG file to hopefully serve as an evolving example of a
  standardized open source project CHANGELOG.

[Unreleased]: https://github.com/reynj/reynj/compare/v1.3.1...HEAD
[1.3.1]: https://github.com/reynj/reynj/compare/v1.3.0...v1.3.1
[1.3.0]: https://github.com/reynj/reynj/compare/v1.2.0...v1.3.0
[1.2.0]: https://github.com/reynj/reynj/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/reynj/reynj/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/reynj/reynj/compare/v0.2.0...v1.0.0
[0.2.0]: https://github.com/reynj/reynj/compare/v0.1.5...v0.2.0
[0.1.5]: https://github.com/reynj/reynj/compare/v0.1.4...v0.1.5
[0.1.4]: https://github.com/reynj/reynj/compare/v0.1.2...v0.1.4
[0.1.3]: https://github.com/reynj/reynj/compare/v0.1.2...v0.1.3