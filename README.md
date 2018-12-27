# Reynj

[![Build Status](https://dev.azure.com/reynj/reynj/_apis/build/status/reynj.reynj?branchName=master)](https://dev.azure.com/reynj/reynj/_build/latest?definitionId=1?branchName=master)
[![NuGet Badge](https://buildstats.info/nuget/Reynj?includePreReleases=false)](https://www.nuget.org/packages/Reynj)

### What is Reynj?

.Net Library that aids in comparison and handling value ranges or time bounded periods.

This implementation is based on the [Range](https://martinfowler.com/eaaDev/Range.html "Martin Fowler Range") class as described by Martin Fowler.

The aim of this library is to provide a base Range class with all possible methods that can be performed on a Range, but also a RangeCollection that can be used to handle and compare list of Ranges.
Below is my list of features I want to implement, feel free to open an issue if something is missing on my list.

- [ ] Range
  - [ ] Implements
    - [x] IEquatable
	- [x] IComparable
	- [ ] ICloneable
  - [x] Operators
     - [x] Equals
	 - [x] CompareTo
	 - [x] Convert to/from Tuple
  - [ ] Methods
    - [x] IsNullOrEmpty
    - [x] Includes & IncludesAll
	- [x] Overlaps
	- [x] Touches
    - [x] Gap
    - [x] Merge (Union)
	- [x] Split
    - [x] Intersection
	- [ ] Enumerate
	- [ ] Expand
  - [ ] Specific implemenations 
    - [ ] DateRange
    - [ ] TimeRange
    - [ ] NumericRange
- [ ] RangeCollection
  - [ ] Methods
    - [ ] Sort
    - [ ] Combine
	- [ ] Reverse
	- [ ] Combine
- [ ] Serialize/Deserialize
  - [ ] JsonConvertor
  - [ ] Entity Framework/NHibernate support
- [x] Other
  - [x] Range<T>.Empty and methods like Merge, Overlaps, Touches, ...
  - [x] IsEmpty method vs Range<T>.Empty

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

#### What is an Empty Range?
Every Range where start and end are equal.

```c#
var range1 = new Range<int>(10, 10);
var range2 = new Range<int>(0, 0);
var range3 = Range<int>.Empty;

// Empty
var equals = range1.Equals(range2); // returns true
var equals = range2.Equals(range3); // returns true
var compare = range1.CompareTo(range3); // returns 0
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

##### Methods
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

###### Overlaps(Range<T> range)
Overlaps will return true if two Ranges overlap. The following example are two overlapping ranges.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBPdmVybGFwc1xuICAgIFxuICAgIFJhbmdlWzAsMTBdICAgICAgICAgICA6IDIwMTgtMDEtMDEuMDAsIDEwaFxuICAgIFJhbmdlWzUsMTVdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wNSwgMTBoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Overlaps](https://mermaid.now.sh//?q=gantt%0A%20%20%20%20dateFormat%20%20YYYY-MM-DD.HH%0A%20%20%20%20axisFormat%20%25-H%0A%20%20%20%20title%20Overlaps%0A%20%20%20%20%0A%20%20%20%20Range%5B0%2C10%5D%20%20%20%20%20%20%20%20%20%20%20%3A%202018-01-01.00%2C%2010h%0A%20%20%20%20Range%5B5%2C15%5D%20%20%20%20%20%20%20%20%20%20%20%3A%20active%2C%202018-01-01.05%2C%2010h)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(5, 15);
var range3 = new Range<int>(15, 25);

// Overlaps
var res1 = range1.Overlaps(range2); // returns true
var res2 = range2.Overlaps(range1); // returns true

var res3 = range1.Overlaps(range3); // returns false
```

###### Touches(Range<T> range)
Touches will return true if two Ranges touch each other. The following example are two touching ranges.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBUb3VjaGVzXG4gICAgXG4gICAgUmFuZ2VbMCw1XSAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCA1aFxuICAgIFJhbmdlWzUsMTBdICAgICAgIDogMjAxOC0wMS0wMS4wNSwgNWgiLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ)
![Overlaps](https://mermaid.now.sh//?q=gantt%0A%20%20%20%20dateFormat%20%20YYYY-MM-DD.HH%0A%20%20%20%20axisFormat%20%25-H%0A%20%20%20%20title%20Touches%0A%20%20%20%20%0A%20%20%20%20Range%5B0%2C5%5D%20%20%20%20%20%20%20%20%20%20%20%3A%202018-01-01.00%2C%205h%0A%20%20%20%20Range%5B5%2C10%5D%20%20%20%20%20%20%20%3A%202018-01-01.05%2C%205h)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(10, 20);
var range3 = new Range<int>(11, 20);

// Overlaps
var res1 = range1.Touches(range2); // returns true
var res2 = range2.Touches(range1); // returns true

var res3 = range1.Touches(range3); // returns false
```

###### Gap(Range<T> range)
Gap returns a new Range that represents the gap between two Ranges.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBHYXBcbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsNV0gICAgICAgICAgIDogMjAxOC0wMS0wMS4wMCwgNWhcbiAgICBSYW5nZVsxMCwxNV0gICAgICAgOiAyMDE4LTAxLTAxLjEwLCA1aFxuXG4gICAgc2VjdGlvbiBHYXBcbiAgICBSYW5nZVs1LDEwXSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDUsIDVoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Gap](https://mermaid.now.sh//?q=https%3A%2F%2Fmermaidjs.github.io%2Fmermaid-live-editor%2F%23%2Fedit%2FeyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBHYXBcbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsNV0gICAgICAgICAgIDogMjAxOC0wMS0wMS4wMCwgNWhcbiAgICBSYW5nZVsxMCwxNV0gICAgICAgOiAyMDE4LTAxLTAxLjEwLCA1aFxuXG4gICAgc2VjdGlvbiBHYXBcbiAgICBSYW5nZVs1LDEwXSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDUsIDVoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(15, 20);

// Gap
var gap1 = range1.Gap(range2); // returns new Range<int>(5, 10)
var gap2 = range2.Gap(range1); // returns new Range<int>(5, 10)
```

###### Merge(Range<T> range)
Merge returns a new Range that represents the combined/merged Range, a [Logical disjunction](https://en.wikipedia.org/wiki/Logical_disjunction).
An exception is thrown when the ranges doe not overlap or touch each other.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBNZXJnZVxuICAgIFxuICAgIHNlY3Rpb24gUmFuZ2VzXG4gICAgUmFuZ2VbMCw1XSAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCA1aFxuICAgIFJhbmdlWzUsMjBdICAgICAgICAgOiAyMDE4LTAxLTAxLjA1LCAxNWhcblxuICAgIHNlY3Rpb24gTWVyZ2VcbiAgICBSYW5nZVswLDIwXSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDAsIDIwaCIsIm1lcm1haWQiOnsidGhlbWUiOiJkZWZhdWx0In19)
![Merge](https://mermaid.now.sh//?width=600&height=400&q=gantt%0A%20%20%20%20dateFormat%20%20YYYY-MM-DD.HH%0A%20%20%20%20axisFormat%20%25-H%0A%20%20%20%20title%20Merge%0A%20%20%20%20%0A%20%20%20%20section%20Ranges%0A%20%20%20%20Range%5B0%2C5%5D%20%20%20%20%20%20%20%20%20%20%20%3A%202018-01-01.00%2C%205h%0A%20%20%20%20Range%5B5%2C20%5D%20%20%20%20%20%20%20%20%20%3A%202018-01-01.05%2C%2015h%0A%0A%20%20%20%20section%20Merge%0A%20%20%20%20Range%5B0%2C20%5D%20%20%20%20%20%20%20%20%20%20%20%3A%20active%2C%202018-01-01.00%2C%2020h)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(5, 20);

// Merge
var merge = range1.Merge(range2); // returns new Range<int>(0, 20)
```

###### Split(Range<T> range)
Split returns a Tuple of two Ranges that have been split on the given value.
An exception is thrown when the value is not included in the Range.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBTcGxpdFxuICAgIFxuICAgIHNlY3Rpb24gUmFuZ2VcbiAgICBSYW5nZVswLDEwXSAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCAxMGhcbiAgICBcblxuICAgIHNlY3Rpb24gU3BsaXRcbiAgICBSYW5nZVswLDVdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wMCwgNWhcbiAgICBSYW5nZVs1LDEwXSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDUsIDVoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Merge](https://mermaid.now.sh//?width=600&height=400&q=gantt%0A%20%20%20%20dateFormat%20%20YYYY-MM-DD.HH%0A%20%20%20%20axisFormat%20%25-H%0A%20%20%20%20title%20Split%0A%20%20%20%20%0A%20%20%20%20section%20Range%0A%20%20%20%20Range%5B0%2C10%5D%20%20%20%20%20%20%20%20%20%20%20%3A%202018-01-01.00%2C%2010h%0A%20%20%20%20%0A%0A%20%20%20%20section%20Split%0A%20%20%20%20Range%5B0%2C5%5D%20%20%20%20%20%20%20%20%20%20%20%3A%20active%2C%202018-01-01.00%2C%205h%0A%20%20%20%20Range%5B5%2C10%5D%20%20%20%20%20%20%20%20%20%20%20%3A%20active%2C%202018-01-01.05%2C%205h)

```c#
var range = new Range<int>(0, 10);

// Split
var split = range.Split(5); // returns (new Range<int>(0, 5), new Range<int>(5, 10))
```

###### Intersection(Range<T> range)
Intersection returns a new Range that represents the the intersection be, a [Logical conjunction](https://en.wikipedia.org/wiki/Logical_conjunction).
An exception is thrown when the ranges doe not overlap each other.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBJbnRlcnNlY3Rpb25cbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsMTBdICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCAxMGhcbiAgICBSYW5nZVs1LDIwXSAgICAgICAgIDogMjAxOC0wMS0wMS4wNSwgMTVoXG5cbiAgICBzZWN0aW9uIEludGVyc2VjdGlvblxuICAgIFJhbmdlWzUsMTBdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wNSwgNWgiLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ)
![Intersection](https://mermaid.now.sh//?width=600&height=400&q=gantt%0A%20%20%20%20dateFormat%20%20YYYY-MM-DD.HH%0A%20%20%20%20axisFormat%20%25-H%0A%20%20%20%20title%20Intersection%0A%20%20%20%20%0A%20%20%20%20section%20Ranges%0A%20%20%20%20Range%5B0%2C10%5D%20%20%20%20%20%20%20%20%20%3A%202018-01-01.00%2C%2010h%0A%20%20%20%20Range%5B5%2C20%5D%20%20%20%20%20%20%20%20%20%3A%202018-01-01.05%2C%2015h%0A%0A%20%20%20%20section%20Intersection%0A%20%20%20%20Range%5B5%2C10%5D%20%20%20%20%20%20%20%20%20%20%20%3A%20active%2C%202018-01-01.05%2C%205h)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(5, 20);

// Intersection
var intersection = range1.Intersection(range2); // returns new Range<int>(5, 10)
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