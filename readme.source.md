# <img src="https://raw.github.com/SimonCropp/XunitLogger/master/icon.png" height="40px"> XunitLogger

Extends [xUnit](https://xunit.net/) to simplify logging.

Redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write) and [Console.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write) to [ITestOutputHelper](https://xunit.net/docs/capturing-output).

Note that [Debug.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug.write) is not redirected since it is not supported on dotnet core.

Uses [AsyncLocal<T>](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1) to track state.


## The NuGet package [![NuGet Status](http://img.shields.io/nuget/v/XunitLogger.svg?style=flat)](https://www.nuget.org/packages/XunitLogger/)

https://nuget.org/packages/XunitLogger/

    PM> Install-Package XunitLogger


## Usage


### ClassBeingTested

snippet: ClassBeingTested.cs


### XunitLoggingBase

`XunitLoggingBase` is an abstract base class for tests. It exposes logging methods for use from unit tests, and handle the flushing of longs in its `Dispose` method. `XunitLoggingBase` is actually a thin wrapper over `XunitLogger`. `XunitLogger`s `Write*` methods can also be use inside a test inheriting from `XunitLoggingBase`.


snippet: TestBaseSample.cs


### XunitLogger


`XunitLogger` provides static access to the logging state for tests. It exposes logging methods for use from unit tests, however flushing of logs must be handled explicitly.

snippet: XunitLoggerSample.cs

`XunitLogger` redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write) and [Console.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write) in its static constructor. These API calls are then routed to the correct Xunit [ITestOutputHelper](https://xunit.net/docs/capturing-output) via a static [AsyncLocal<T>](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1).

snippet: writRedirects


## Icon

<a href="http://thenounproject.com/term/wolverine/18415/" target="_blank">Wolverine</a> designed by <a href="https://thenounproject.com/itsmikerowe/" target="_blank">Mike Rowe</a> from The Noun Project
