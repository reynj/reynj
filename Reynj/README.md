# Reynj

[![Build](https://github.com/reynj/reynj/actions/workflows/build.yml/badge.svg)](https://github.com/reynj/reynj/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/reynj?logo=nuget&logoSize=auto)](https://www.nuget.org/packages/Reynj)

### What is Reynj?

.Net Library that aids in comparison and handling value ranges or time bounded periods. Other names for a Range are Interval, Sequence, Period, Span, ... 

This implementation is based on the [Range](https://martinfowler.com/eaaDev/Range.html "Martin Fowler Range") class as described by Martin Fowler.

The aim of this library is to provide a base Range class with all possible methods that can be performed on a Range, but also extension methods that can be used to handle and compare lists of Ranges.

### Where can I get it?

You can install [Reynj with NuGet](https://www.nuget.org/packages/Reynj):

    Install-Package Reynj
    
Or via the .NET Core command line interface:

    dotnet add package Reynj

Either commands, from Package Manager Console or .NET Core CLI, will download and install Reynj and all required dependencies.

### How to use it?
#### What is a Range?
A Range is best visualized as a bar. It has a start and an end and contains everything between those two. Below is a visualization of Range of integers that start at 0 and end at 10. All whole numbers between 0 and 10 are included in the Range, except 10.

To create this Range in code, you can do the following:

```c#
var range = new Range<int>(0, 10);
```

There is only one limitation, the start of Range must be lower or equal to the end.

More information about the Range class and its methods can be found in the [documentation](https://github.com/reynj/reynj/blob/master/README.md) on GitHub.