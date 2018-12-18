# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added
- The Includes method was added to Range<T>, it returns true if the the given value is a part of the Range.

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

[Unreleased]: https://github.com/reynj/reynj/compare/0.1.3...HEAD
[0.1.3]: https://github.com/reynj/reynj/compare/0.1.2...0.1.3