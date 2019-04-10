<!--
This file was generate by MarkdownSnippets.
Source File: /readme.source.md
To change this file edit the source file and then re-run the generation using either the dotnet global tool (https://github.com/SimonCropp/MarkdownSnippets#githubmarkdownsnippets) or using the api (https://github.com/SimonCropp/MarkdownSnippets#running-as-a-unit-test).
-->
# <img src="https://raw.github.com/SimonCropp/XunitLogger/master/icon.png" height="40px"> XunitLogger

Extends [xUnit](https://xunit.net/) to simplify logging.

Redirects `Trace.Write*` and `Console.Write*` to `ITestOutputHelper`.

Uses [AsyncLocal<T>](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1) to track state.


## The NuGet package [![NuGet Status](http://img.shields.io/nuget/v/XunitLogger.svg?style=flat)](https://www.nuget.org/packages/XunitLogger/)

https://nuget.org/packages/XunitLogger/

    PM> Install-Package XunitLogger


## Usage


### ClassBeingTested

<!-- snippet: ClassBeingTested.cs -->
```cs
using System;
using System.Diagnostics;

static class ClassBeingTested
{
    public static void Method()
    {
        Trace.WriteLine("From Trace");
        Console.WriteLine("From Console");
    }
}
```
<sup>[snippet source](/src/Tests/Snippets/ClassBeingTested.cs#L1-L11)</sup>
<!-- endsnippet -->


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
        WriteLine("From Test");
        ClassBeingTested.Method();

        var logs = XunitLogger.Logs;

        Assert.Contains("From Test", logs);
        Assert.Contains("From Trace", logs);
        Assert.Contains("From Console", logs);
    }

    public TestBaseSample(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}
```
<sup>[snippet source](/src/Tests/Snippets/TestBaseSample.cs#L1-L24)</sup>
<!-- endsnippet -->


### XunitLogger

<!-- snippet: XunitLoggerSample.cs -->
```cs
using System;
using Xunit;
using Xunit.Abstractions;

public class XunitLoggerSample : 
    IDisposable
{
    [Fact]
    public void Usage()
    {
        XunitLogger.WriteLine("From Test");

        ClassBeingTested.Method();

        var logs = XunitLogger.Logs;

        Assert.Contains("From Test", logs);
        Assert.Contains("From Trace", logs);
        Assert.Contains("From Console", logs);
    }

    public XunitLoggerSample(ITestOutputHelper testOutput)
    {
        XunitLogger.Register(testOutput);
    }

    public void Dispose()
    {
        XunitLogger.Flush();
    }
}
```
<sup>[snippet source](/src/Tests/Snippets/XunitLoggerSample.cs#L1-L31)</sup>
<!-- endsnippet -->


## Icon

<a href="http://thenounproject.com/term/wolverine/18415/" target="_blank">Wolverine</a> designed by <a href="https://thenounproject.com/itsmikerowe/" target="_blank">Mike Rowe</a> from The Noun Project
