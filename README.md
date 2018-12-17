# reynj

[![Build Status](https://dev.azure.com/reynj/reynj/_apis/build/status/reynj.reynj?branchName=master)](https://dev.azure.com/reynj/reynj/_build/latest?definitionId=1?branchName=master)

.Net Library that aids in comparison and handling values ranges or time bounded periods.

This implementation is based on the [Range](https://martinfowler.com/eaaDev/Range.html "Martin Fowler Range") class as described by Martin Fowler.

The class itself is easily copied around and does not need a library to pull via NuGet into your project. But my aim is to also add other features on top of the Range class, ideas are:

* Specific implemenations like DateRange, TimeRange, NumericRange
* Functions like Union, Intersect, CompareTo, ...
* Collections of Ranges with ways to remove doubles and overlapping ranges, sorting, combining, reversing, ...
* JsonConvertor so that ranges can be used in WebApis
* Entity Framework and NHibernate support