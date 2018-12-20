# Reynj

[![Build Status](https://dev.azure.com/reynj/reynj/_apis/build/status/reynj.reynj?branchName=master)](https://dev.azure.com/reynj/reynj/_build/latest?definitionId=1?branchName=master)
[![NuGet Badge](https://buildstats.info/nuget/Reynj?includePreReleases=true)](https://www.nuget.org/packages/Reynj)

### What is Reynj?

.Net Library that aids in comparison and handling values ranges or time bounded periods.

This implementation is based on the [Range](https://martinfowler.com/eaaDev/Range.html "Martin Fowler Range") class as described by Martin Fowler.

The class itself is easily copied around and does not need a library to pull via NuGet into your project. But my aim is to also add other features on top of the Range class, ideas are:

* Specific implemenations like DateRange, TimeRange, NumericRange
* Functions like Union, Intersect, CompareTo, ...
* Collections of Ranges with ways to remove doubles and overlapping ranges, sorting, combining, reversing, ...
* JsonConvertor so that ranges can be used in WebApis
* Entity Framework and NHibernate support

### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Reynj](https://www.nuget.org/packages/Reynj/) from the package manager console:

```
PM> Install-Package Reynj
```

### How to use it?
#### What is a Range?
A Range is best visualized as a bar. It has a start and en end and contains everything between those two. Below is a visualization of Range of integers that start at 0 and end at 10. All whole numbers between 0 and 10 are included in the Range, except 10.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBSYW5nZTxpbnQ-XG4gICAgXG4gICAgUmFuZ2VbMCwxMF0gICAgICAgICAgIDogMjAxOC0wMS0wMS4wMCwgMTBoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Range<int>](https://mermaid.now.sh//?q=gantt%0A%20%20%20%20dateFormat%20%20YYYY-MM-DD.HH%0A%20%20%20%20axisFormat%20%25-H%0A%20%20%20%20title%20Range%3Cint%3E%0A%20%20%20%20%0A%20%20%20%20Range%5B0%2C10%5D%20%20%20%20%20%20%20%20%20%20%20%3A%202018-01-01.00%2C%2010h)

To create this Range in code, you can do the following:

```c#
var range = new Range<int>(0, 10);
```

There is only one limitation, the start of Range must be lower or equal to the end.

#### What are the types of Ranges that can be created?
The type of Range must derive from the [IComparable<T>](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1?view=netcore-2.2) interface. Below some common examples.
  
```c#
// Numeric Ranges
var intRange = new Range<int>(0, 10);
var doubleRange = new Range<double>(0.0, 0.5);

// Date and Time Ranges
var dateRange = new Range<DateTime>(new DateTime(2018, 12, 18), new DateTime(2018, 12, 25));
var doubleRange = new Range<TimeSpan>(TimeSpan.FromHours(0), TimeSpan.FromHours(6));
```

#### What can be done with a Range?
##### Determining equality
Because Range<T> implements the [IEquatable<T>](https://docs.microsoft.com/en-us/dotnet/api/system.iequatable-1?view=netcore-2.2) interface, including the operators the following can be done:
  
```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(0, 10);
var range3 = new Range<int>(5, 9);

// Equals
var res1 = range1.Equals(range2); // returns true
var res2 = range1.Equals(range3); // returns false

// Equality Operators
var res3 = range1 == range2; // returns true
var res4 = range1 != range2; // returns false
var res5 = range1 == range3; // returns false
var res6 = range1 != range3; // returns true
```

##### Ordering or Sorting
Because Range<T> implements the [IComparable<T>](https://docs.microsoft.com/en-us/dotnet/api/system.icomparable-1?view=netcore-2.2) interface, including the operators the following can be done:
  
```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(5, 9);

// CompareTo
var res1 = range1.CompareTo(range2);

// Equality Operators
var res2 = range1 < range2;
var res3 = range1 > range2;
var res4 = range1 <= range2;
var res5 = range1 >= range2;
```

##### Tuples
A Range<T> has two primary properties, Start and End, because of that a Range<T> can also be represented as a Tuple, in the lastest versions of .Net as a [ValueTuple](https://docs.microsoft.com/en-us/dotnet/api/system.valuetuple?view=netcore-2.2).
The constructor of a Range<T> accepts a Tuple of two elements and there is an AsTuple method to convert a Range<T> to a ValueTuple<T, T>.
Conversion operators have been implemented to makes this even more smoothly.

```c#
var tuple = (0, 10);
var range = new Range<int>(tuple);

// AsTuple
var otherTuple = range.AsTuple();

// Conversion Operators
var otherTuple2 = range; // implicit from Range to Tuple
var otherRange = (Range<int>) tuple; // explicit from Tuple to Range

```

##### Other functions
###### Includes(T value), Includes(Range<T> range) and IncludesAll(IEnumerable<T> values)
Includes will return true if the given value is a part of the Range, otherwise false.
IncludesAll will return true if all of the given values are part of the Range, otherwise false.

```c#
var range = new Range<int>(0, 10);

// Includes(T value)
var res1 = range1.Includes(5); // returns true
var res2 = range1.Includes(20); // returns false

// Includes(Range<T> range)
var res1 = range1.Includes(new Range<int>(2, 7)); // returns true
var res2 = range1.Includes(new Range<int>(20, 30)); // returns false

// IncludesAll
var res3 = range1.IncludesAll(0, 1, 2, 3, 4, 5, 6, 7, 8, 9); // returns true
var res4 = range1.IncludesAll(0, 1, 2, 3, 4, 20, 6, 7, 8, 9); // returns false
```

###### IsEmpty()
IsEmpty will return true if the start and the end of the Range are equal, otherwise false.

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(10, 10);

// IsEmpty
var res1 = range1.IsEmpty(); // returns false
var res2 = range2.IsEmpty(); // returns true
```