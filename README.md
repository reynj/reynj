# Reynj

[![Build Status](https://dev.azure.com/reynj/reynj/_apis/build/status/reynj.reynj?branchName=master)](https://dev.azure.com/reynj/reynj/_build/latest?definitionId=1?branchName=master)
[![NuGet Badge](https://buildstats.info/nuget/Reynj?includePreReleases=false)](https://www.nuget.org/packages/Reynj)

### What is Reynj?

.Net Library that aids in comparison and handling value ranges or time bounded periods.

This implementation is based on the [Range](https://martinfowler.com/eaaDev/Range.html "Martin Fowler Range") class as described by Martin Fowler.

The aim of this library is to provide a base Range class with all possible methods that can be performed on a Range, but also extension methods that can be used to handle and compare list of Ranges.
Below is my list of features I want to implement, feel free to open an issue if something is missing on my list.

<details>
  <summary>Click to expand the list</summary>

    - [ ] Range
      - [ ] Boundaries
      - [x] Implements
        - [x] IEquatable
	    - [x] IComparable
	    - [x] ICloneable
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
	    - [x] Exclusive
	    - [ ] Enumerate
	    - [ ] Expand
      - [ ] Specific implemenations 
        - [ ] DateRange or Period
        - [ ] TimeRange
        - [ ] NumericRange
    - [ ] Collection of Ranges
      - [ ] Methods
        - [x] Lowest/Highest
	    - [x] IsContiguous
        - [ ] ToRange (only possible for a Contiguous collection)
        - [ ] IsSingle
	    - [x] Reduce
        - [x] Sort
	    - [x] Union
	    - [x] Intersect
        - [x] Inverse
	    - [ ] Difference (Relative complement)
	    - [ ] Exclusive
    - [ ] Serialize/Deserialize
      - [ ] JsonConvertor
      - [ ] Entity Framework/NHibernate support
    - [ ] Other
      - [x] Range<T>.Empty and methods like Merge, Overlaps, Touches, ...
      - [x] IsEmpty method vs Range<T>.Empty
      - [x] Support for conversion between System.Range (C# 8.0) and Range<int>
</details>


### Where can I get it?

First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Reynj](https://www.nuget.org/packages/Reynj/) from the package manager console:

```
PM> Install-Package Reynj
```

### How to use it?
#### What is a Range?
A Range is best visualized as a bar. It has a start and an end and contains everything between those two. Below is a visualization of Range of integers that start at 0 and end at 10. All whole numbers between 0 and 10 are included in the Range, except 10.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBSYW5nZTxpbnQ-XG4gICAgXG4gICAgUmFuZ2VbMCwxMF0gICAgICAgICAgIDogMjAxOC0wMS0wMS4wMCwgMTBoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Range<int>](./images/range.svg)

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
var dateRange = new Range<DateTime>(new DateTime(2018, 12, 18), new DateTime(2018, 12, 25)); // Period
var timeSpanRange = new Range<TimeSpan>(TimeSpan.FromHours(0), TimeSpan.FromHours(6)); // Duration
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
var res1 = range1.CompareTo(range2); // returns -1

// Equality Operators
var res2 = range1 < range2;  // returns true
var res3 = range1 > range2;  // returns false
var res4 = range1 <= range2; // returns true
var res5 = range1 >= range2; // returns false
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
![Overlaps](./images/overlaps.svg)

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
![Touches](./images/touches.svg)

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
![Gap](./images/gap.svg)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(15, 20);

// Gap
var gap1 = range1.Gap(range2); // returns new Range<int>(5, 10)
var gap2 = range2.Gap(range1); // returns new Range<int>(5, 10)
```

###### Merge(Range<T> range)
Merge returns a new Range that represents the combined/merged Range, a [Logical disjunction](https://en.wikipedia.org/wiki/Logical_disjunction).
An exception is thrown when the ranges do not overlap or touch each other.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBNZXJnZVxuICAgIFxuICAgIHNlY3Rpb24gUmFuZ2VzXG4gICAgUmFuZ2VbMCw1XSAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCA1aFxuICAgIFJhbmdlWzUsMjBdICAgICAgICAgOiAyMDE4LTAxLTAxLjA1LCAxNWhcblxuICAgIHNlY3Rpb24gTWVyZ2VcbiAgICBSYW5nZVswLDIwXSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDAsIDIwaCIsIm1lcm1haWQiOnsidGhlbWUiOiJkZWZhdWx0In19)
![Merge](./images/merge.svg)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(5, 20);

// Merge
var merge1 = range1.Merge(range2); // returns new Range<int>(0, 20)
var merge2 = range1 | range2; // returns new Range<int>(0, 20)
```

###### Split(Range<T> range)
Split returns a Tuple of two Ranges that have been split on the given value.
An exception is thrown when the value is not included in the Range.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBTcGxpdFxuICAgIFxuICAgIHNlY3Rpb24gUmFuZ2VcbiAgICBSYW5nZVswLDEwXSAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCAxMGhcbiAgICBcblxuICAgIHNlY3Rpb24gU3BsaXRcbiAgICBSYW5nZVswLDVdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wMCwgNWhcbiAgICBSYW5nZVs1LDEwXSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDUsIDVoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Split](./images/split.svg)

```c#
var range = new Range<int>(0, 10);

// Split
var split = range.Split(5); // returns (new Range<int>(0, 5), new Range<int>(5, 10))
```

###### Intersection(Range<T> range)
Intersection returns a new Range that represents the intersection between the current Range and a given Range, a [Logical conjunction](https://en.wikipedia.org/wiki/Logical_conjunction).
An exception is thrown when the ranges do not overlap each other.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBJbnRlcnNlY3Rpb25cbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsMTBdICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCAxMGhcbiAgICBSYW5nZVs1LDIwXSAgICAgICAgIDogMjAxOC0wMS0wMS4wNSwgMTVoXG5cbiAgICBzZWN0aW9uIEludGVyc2VjdGlvblxuICAgIFJhbmdlWzUsMTBdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wNSwgNWgiLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ)
![Intersection](./images/intersection.svg)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(5, 20);

// Intersection
var intersection1 = range1.Intersection(range2); // returns new Range<int>(5, 10)
var intersection2 = range1 & range2; // returns new Range<int>(5, 10)
```

###### Exclusive(Range<T> range)
Exclusive returns a tuple of Ranges that that represent the parts they do not have in common, an [Exclusive or](https://en.wikipedia.org/wiki/Exclusive_or).
An exception is thrown when the ranges are null or Empty.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBFeGNsdXNpdmVcbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsMTBdICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCAxMGhcbiAgICBSYW5nZVs1LDIwXSAgICAgICAgIDogMjAxOC0wMS0wMS4wNSwgMTVoXG5cbiAgICBzZWN0aW9uIEV4Y2x1c2l2ZVxuICAgIFJhbmdlWzAsIDVdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wMCwgNWhcbiAgICBSYW5nZVsxMCwgMjBdICAgICAgIDogYWN0aXZlLCAyMDE4LTAxLTAxLjEwLCAxMGgiLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ)
![Exclusive](./images/exclusive.svg)

```c#
var range1 = new Range<int>(0, 10);
var range2 = new Range<int>(5, 20);

// Exclusive
var exclusive = range1.Exlusive(range2); // returns (new Range<int>(0, 5), new Range<int>(10, 20))
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

###### ToRange()
With the ToRange extension methods on both Range<int> and System.Range a conversion can be done between them.
A System.Range and a Reynj.Range have not so much in common, a [System.Range](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-8#indices-and-ranges) is meant for accessing single elements or ranges in a sequence,
but by converting from them to a Range<int> it is possible to use all methods on Range<int> and Enumerable<Range<int>>.


```c#
var range = new Range<int>(0, 10);

// ToRange()
var sysRange = range.ToRange(); // returns new System.Range(0, 10)

// ToRange()
var reynjRange = sysRange.ToRange(); // returns new Range<int>(0, 10)
```


#### What is a Collection of Ranges?
A Collection of Ranges is a group or list of Ranges of the same type. 

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBSYW5nZUNvbGxlY3Rpb25cbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMDAsIDVoXG4gICAgUmFuZ2VbNywxMF0gICAgICAgICA6IDIwMTgtMDEtMDEuMDcsIDNoXG4gICAgUmFuZ2VbMTAsMTVdICAgICAgICAgOiAyMDE4LTAxLTAxLjEwLCA1aFxuICAgIFJhbmdlWzE4LDI1XSAgICAgICAgIDogMjAxOC0wMS0wMS4xOCwgN2giLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ)
![CollectionOfRanges](./images/collection.svg)

To create an IEnumerable<Range<T>> in code, you can do the following:

```c#
// Collection based on an IEnumerable<Range<T>>
var ranges = new List<Range<int>>() {
    new Range<int>(0, 10),
    new Range<int>(10, 20)
}
```

#### What can be done with a Collection of Ranges?

##### Extension Methods
###### Lowest() / Highest()
They return the lowest start or highest end of the all the Ranges in the collection.

```c#
var ranges = new List<Range<int>>
{
    new Range<int>(0, 10),
    new Range<int>(10, 20)
};

// Lowest
var lowest = ranges.Lowest(); // returns 0

// Highest
var highest = ranges.Highest(); // returns 20
```

###### Reduce()
Returns a new Collection of Ranges where all overlapping and touching Ranges have been merged and empty Ranges have been removed.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBSZWR1Y2VcbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMDAsIDVoXG4gICAgUmFuZ2VbMywxMF0gICAgICAgICA6IDIwMTgtMDEtMDEuMDMsIDdoXG4gICAgUmFuZ2VbMTAsMTVdICAgICAgICAgOiAyMDE4LTAxLTAxLjEwLCA1aFxuICAgIFJhbmdlWzE4LDI1XSAgICAgICAgIDogMjAxOC0wMS0wMS4xOCwgN2hcblxuICAgIHNlY3Rpb24gUmVkdWNlZFxuICAgIFJhbmdlWzAsMTVdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wMCwgMTVoXG4gICAgUmFuZ2VbMTgsMjVdICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMTgsIDdoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Reduce](./images/reduce.svg)

```c#
var ranges = new[]
{
    new Range<int>(0, 5),
    new Range<int>(3, 10),
    new Range<int>(10, 15),
    new Range<int>(18, 25)
});

// Reduce
var reduced = ranges.Reduce(); // returns new[] { new Range<int>(0, 15), new Range<int>(18, 25) }
```

###### Union()
Returns the [union](https://en.wikipedia.org/wiki/Union_(set_theory)) of two Collections of Ranges while reducing them.
An exception is thrown when one or both of the ranges are null.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBVbmlvblxuICAgIFxuICAgIHNlY3Rpb24gUmFuZ2VzIDFcbiAgICBSYW5nZVswLDVdICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCA1aFxuICAgIFJhbmdlWzMsMTBdICAgICAgICAgOiAyMDE4LTAxLTAxLjAzLCA3aFxuICAgIFJhbmdlWzEwLDE1XSAgICAgICAgIDogMjAxOC0wMS0wMS4xMCwgNWhcblxuXG4gICAgc2VjdGlvbiBSYW5nZXMgMlxuICAgIFJhbmdlWzE1LDE3XSAgICAgICAgIDogMjAxOC0wMS0wMS4xNSwgMmhcbiAgICBSYW5nZVsxOCwyNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMTgsIDdoXG5cbiAgICBzZWN0aW9uIFVuaW9uZWRcbiAgICBSYW5nZVswLDE3XSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDAsIDE3aFxuICAgIFJhbmdlWzE4LDI1XSAgICAgICAgIDogYWN0aXZlLCAyMDE4LTAxLTAxLjE4LCA3aCIsIm1lcm1haWQiOnsidGhlbWUiOiJkZWZhdWx0In19)
![Reduce](./images/union.svg)

```c#
var ranges1 = new[]
{
    new Range<int>(0, 5),
    new Range<int>(3, 10),
    new Range<int>(10, 15)
});

var ranges2 = new[]
{
    new Range<int>(15, 17),
    new Range<int>(18, 25)
});

// Union
var unioned = ranges1.Union(ranges2); // returns new[] { new Range<int>(0, 17), new Range<int>(18, 25) }
```

###### Intersect()
Returns the [intersection](https://en.wikipedia.org/wiki/Intersection_(set_theory)) of two Collections of Ranges while reducing them.
An exception is thrown when one or both of the ranges are null.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBJbnRlcnNlY3RcbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlcyAxXG4gICAgUmFuZ2VbMCw1XSAgICAgICAgICAgICA6IDIwMTgtMDEtMDEuMDAsIDVoXG4gICAgUmFuZ2VbMywxMF0gICAgICAgICAgIDogMjAxOC0wMS0wMS4wMywgN2hcbiAgICBSYW5nZVsxMCwxNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMTAsIDVoXG4gICAgUmFuZ2VbMTgsMjBdICAgICAgICAgOiAyMDE4LTAxLTAxLjE4LCAyaFxuXG4gICAgc2VjdGlvbiBSYW5nZXMgMlxuICAgIFJhbmdlWzEsOF0gICAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAxLCA3aFxuICAgIFJhbmdlWzEyLDI1XSAgICAgICAgIDogMjAxOC0wMS0wMS4xMiwgMTNoXG5cbiAgICBzZWN0aW9uIEludGVyc2VjdGlvblxuICAgIFJhbmdlWzEsOF0gICAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDEsIDdoXG4gICAgUmFuZ2VbMTIsMTVdICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMTIsIDNoXG4gICAgUmFuZ2VbMTgsMjBdICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMTgsIDJoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Reduce](./images/intersect.svg)

```c#
var ranges1 = new[]
{
    new Range<int>(0, 5),
    new Range<int>(3, 10),
    new Range<int>(10, 15),
    new Range<int>(18, 20)
});

var ranges2 = new[]
{
    new Range<int>(1, 8),
    new Range<int>(12, 25)
});

// Intersect
var intersection = ranges1.Intersect(ranges2); // returns new[] { new Range<int>(1, 8), new Range<int>(12, 15), new Range<int>(18, 20) }
```

<!--
###### Difference
Returns the set difference or [relative complement](https://en.wikipedia.org/wiki/Complement_(set_theory)#Relative_complement) of two Collections of Ranges while reducing them.
An exception is thrown when one or both of the ranges are null.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBEaWZmZXJlbmNlXG4gICAgXG4gICAgc2VjdGlvbiBSYW5nZXMgMVxuICAgIFJhbmdlWzAsNV0gICAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAwLCA1aFxuICAgIFJhbmdlWzMsMTBdICAgICAgICAgICA6IDIwMTgtMDEtMDEuMDMsIDdoXG4gICAgUmFuZ2VbMTAsMTVdICAgICAgICAgOiAyMDE4LTAxLTAxLjEwLCA1aFxuICAgIFJhbmdlWzE4LDIwXSAgICAgICAgIDogMjAxOC0wMS0wMS4xOCwgMmhcblxuICAgIHNlY3Rpb24gUmFuZ2VzIDJcbiAgICBSYW5nZVsxLDhdICAgICAgICAgICAgIDogMjAxOC0wMS0wMS4wMSwgN2hcbiAgICBSYW5nZVsxMiwyNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMTIsIDEzaFxuXG4gICAgc2VjdGlvbiBEaWZmZXJlbmNlT2ZcbiAgICBSYW5nZVsxNSwxOF0gICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4xNSwgM2hcbiAgICBSYW5nZVsyMCwyNV0gICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4yMCwgNWgiLCJtZXJtYWlkIjp7InRoZW1lIjoiZGVmYXVsdCJ9fQ)
![Reduce](./images/difference.svg)

```c#
var ranges1 = new[]
{
    new Range<int>(0, 5),
    new Range<int>(3, 10),
    new Range<int>(10, 15),
    new Range<int>(18, 20)
});

var ranges2 = new[]
{
    new Range<int>(1, 8),
    new Range<int>(12, 25)
});

// Set Difference
var differenceOf = ranges1.Difference(ranges2); // returns new[] { new Range<int>(15, 18), new Range<int>(20, 25) }
```

###### Exclusive()
Returns the [exclusive or](https://en.wikipedia.org/wiki/Symmetric_difference) of two Collections of Ranges while reducing them.
An exception is thrown when one or both of the ranges are null.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBFeGNsdXNpdmVcbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlcyAxXG4gICAgUmFuZ2VbMCw1XSAgICAgICAgICAgICA6IDIwMTgtMDEtMDEuMDAsIDVoXG4gICAgUmFuZ2VbMywxMF0gICAgICAgICAgIDogMjAxOC0wMS0wMS4wMywgN2hcbiAgICBSYW5nZVsxMCwxNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMTAsIDVoXG4gICAgUmFuZ2VbMTgsMjBdICAgICAgICAgOiAyMDE4LTAxLTAxLjE4LCAyaFxuXG4gICAgc2VjdGlvbiBSYW5nZXMgMlxuICAgIFJhbmdlWzEsOF0gICAgICAgICAgICAgOiAyMDE4LTAxLTAxLjAxLCA3aFxuICAgIFJhbmdlWzEyLDI1XSAgICAgICAgIDogMjAxOC0wMS0wMS4xMiwgMTNoXG5cbiAgICBzZWN0aW9uIEV4Y2x1c2l2ZU9mXG4gICAgUmFuZ2VbMCwxXSAgICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wMCwgMWhcbiAgICBSYW5nZVs4LDEyXSAgICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDgsIDRoXG4gICAgUmFuZ2VbMTUsMThdICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMTUsIDNoXG4gICAgUmFuZ2VbMjAsMjVdICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMjAsIDVoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Reduce](./images/collection-exclusive.svg)

```c#
var ranges1 = new[]
{
    new Range<int>(0, 5),
    new Range<int>(3, 10),
    new Range<int>(10, 15),
    new Range<int>(18, 20)
});

var ranges2 = new[]
{
    new Range<int>(1, 8),
    new Range<int>(12, 25)
});

// Intersect
var exclusiveOr = ranges1.Exclusive(ranges2); // returns new[] { new Range<int>(0, 1), new Range<int>(8, 12), new Range<int>(15, 18), new Range<int>(20, 25) }
```
-->

###### Inverse()
Returns a new Collection of Ranges that is the inversion of the Ranges. Meaning all gaps between the ranges are returned including the gap between the minvalue and the first start and the last end and the maxvalue.
Also known as the [absolute complement](https://en.wikipedia.org/wiki/Complement_(set_theory)#Absolute_complement).
An exception is thrown when the type of Range has no MinValue and MaxValue or when they are not passed.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBJbnZlcnNlXG4gICAgXG4gICAgc2VjdGlvbiBSYW5nZXNcbiAgICBSYW5nZVswLDVdICAgICAgICAgICAgIDogMjAxOC0wMS0wMS4wMCwgNWhcbiAgICBSYW5nZVsxMCwxNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMTAsIDVoXG4gICAgUmFuZ2VbMTgsMjBdICAgICAgICAgOiAyMDE4LTAxLTAxLjE4LCAyaFxuXG4gICAgc2VjdGlvbiBJbnZlcnNpb25cbiAgICBSYW5nZVst4oieLCAwXSAgICAgICAgIDogYWN0aXZlLCAyMDE3LTEyLTMxLjIxLCAzaFxuICAgIFJhbmdlWzUsMTBdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wNSwgNWhcbiAgICBSYW5nZVsxNSwyOF0gICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4xNSwgM2hcbiAgICBSYW5nZVsyMCwr4oieXSAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMjAsIDNoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0)
![Reduce](./images/inverse.svg)

```c#
var ranges = new[]
{
    new Range<int>(0, 5),
    new Range<int>(10, 15),
    new Range<int>(18, 20)
});

// Inverse
var inversion = ranges.Inverse(); // returns new[] { new Range<int>(int.MinValue, 0), new Range<int>(5, 10), new Range<int>(15, 18), new Range<int>(20, int.MaxValue) }
```

###### IsContiguous()
Check if a collection of Ranges only contains touching Ranges and form a contiguous sequence.

[//]: # (Mermaid: https://mermaidjs.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBSZWR1Y2VcbiAgICBcbiAgICBzZWN0aW9uIFJhbmdlc1xuICAgIFJhbmdlWzAsNV0gICAgICAgICA6IDIwMTgtMDEtMDEuMDAsIDVoXG4gICAgUmFuZ2VbMywxMF0gICAgICAgICA6IDIwMTgtMDEtMDEuMDMsIDdoXG4gICAgUmFuZ2VbMTAsMTVdICAgICAgICAgOiAyMDE4LTAxLTAxLjEwLCA1aFxuICAgIFJhbmdlWzE4LDI1XSAgICAgICAgIDogMjAxOC0wMS0wMS4xOCwgN2hcblxuICAgIHNlY3Rpb24gUmVkdWNlZFxuICAgIFJhbmdlWzAsMTVdICAgICAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4wMCwgMTVoXG4gICAgUmFuZ2VbMTgsMjVdICAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMTgsIDdoIiwibWVybWFpZCI6eyJ0aGVtZSI6ImRlZmF1bHQifX0https://mermaid-js.github.io/mermaid-live-editor/#/edit/eyJjb2RlIjoiZ2FudHRcbiAgICBkYXRlRm9ybWF0ICBZWVlZLU1NLURELkhIXG4gICAgYXhpc0Zvcm1hdCAlLUhcbiAgICB0aXRsZSBJc0NvbnRpZ3VvdXNcbiAgICBcbiAgICBzZWN0aW9uIElzIENvbnRpZ3VvdXNcbiAgICBSYW5nZVszLDEwXSAgICAgICAgIDogMjAxOC0wMS0wMS4wMywgN2hcbiAgICBSYW5nZVsxMCwxNV0gICAgICAgIDogMjAxOC0wMS0wMS4xMCwgNWhcblxuICAgIHNlY3Rpb24gTm90IENvbnRpZ3VvdXMgKG92ZXJsYXApXG4gICAgUmFuZ2VbMCw1XSAgICAgICAgIDogY3JpdCwgZG9uZSwgMjAxOC0wMS0wMS4wMCwgNWhcbiAgICBSYW5nZVszLDEwXSAgICAgICAgOiBjcml0LCBkb25lLCAyMDE4LTAxLTAxLjAzLCA3aFxuICAgIFJhbmdlWzEwLDE1XSAgICAgICA6IGFjdGl2ZSwgMjAxOC0wMS0wMS4xMCwgNWhcblxuICAgIHNlY3Rpb24gTm90IENvbnRpZ3VvdXMgKGdhcClcbiAgICBSYW5nZVszLDEwXSAgICAgICAgOiBhY3RpdmUsIDIwMTgtMDEtMDEuMDMsIDdoXG4gICAgUmFuZ2VbMTAsMTVdICAgICAgIDogY3JpdCwgZG9uZSwgMjAxOC0wMS0wMS4xMCwgNWhcbiAgICBSYW5nZVsxOCwyNV0gICAgICAgOiBjcml0LCBkb25lLCAyMDE4LTAxLTAxLjE4LCA3aCIsIm1lcm1haWQiOnsidGhlbWUiOiJkZWZhdWx0In0sInVwZGF0ZUVkaXRvciI6ZmFsc2V9)
![Reduce](./images/iscontiguous.svg)

```c#
var contiguousRanges = new[]
{
    new Range<int>(10, 15),
    new Range<int>(18, 25)
});

var withOverlapRanges = new[]
{
    new Range<int>(0, 5),
    new Range<int>(3, 10),
    new Range<int>(10, 15)
});

var notTouchingRanges = new[]
{
    new Range<int>(3, 10),
    new Range<int>(10, 15),
    new Range<int>(18, 25)
});

// IsContiguous
var isContiguous1 = contiguousRanges.IsContiguous(); // returns true
var isContiguous2 = withOverlapRanges.IsContiguous(); // returns false
var isContiguous3 = notTouchingRanges.IsContiguous(); // returns false
```