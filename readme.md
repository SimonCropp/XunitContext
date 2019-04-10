<!--
This file was generate by MarkdownSnippets.
Source File: /readme.source.md
To change this file edit the source file and then re-run the generation using either the dotnet global tool (https://github.com/SimonCropp/MarkdownSnippets#githubmarkdownsnippets) or using the api (https://github.com/SimonCropp/MarkdownSnippets#running-as-a-unit-test).
-->
# <img src="https://raw.github.com/SimonCropp/XunitLogger/master/icon.png" height="40px"> XunitLogger

Extends [xUnit](https://xunit.net/) to simplify logging.


## The NuGet package [![NuGet Status](http://img.shields.io/nuget/v/XunitLogger.svg?style=flat)](https://www.nuget.org/packages/XunitLogger/)

https://nuget.org/packages/XunitLogger/

    PM> Install-Package XunitLogger


## Usage


### XunitLoggingBase

<!-- snippet: TestBaseSample.cs -->
```cs
using Xunit;
using Xunit.Abstractions;

public class TestBaseSample :
    XunitLoggingBase
{
    [Fact]
    public void Write_lines()
    {
        WriteLine("The log message");
    }

    public TestBaseSample(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}
```
<sup>[snippet source](/src/Tests/TestBaseSample.cs#L1-L17)</sup>
<!-- endsnippet -->


### XunitLogger


## Icon

<a href="http://thenounproject.com/term/wolverine/18415/" target="_blank">Wolverine</a> designed by <a href="https://thenounproject.com/itsmikerowe/" target="_blank">Mike Rowe</a> from The Noun Project
