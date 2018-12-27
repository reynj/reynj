# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- A static IsNullOrEmpty method, to check if a Range<T> is null or Range<T>.Empty
- The Merge method, to create a single Range<T> from two overlapping or touching Ranges
- The Split method, to split the Range<T> at a given value
- The Intersection method, that returns the Range<T> that is part of both Ranges

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

[Unreleased]: https://github.com/reynj/reynj/compare/v0.1.4...HEAD
[0.1.4]: https://github.com/reynj/reynj/compare/v0.1.2...v0.1.4
[0.1.3]: https://github.com/reynj/reynj/compare/v0.1.2...v0.1.3