# <img src="/src/icon.png" height="40px"> XunitLogger

Extends [xUnit](https://xunit.net/) to simplify logging.

Redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write), [Debug.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug.write), and [Console.Write and Console.Error.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write) to [ITestOutputHelper](https://xunit.net/docs/capturing-output). Also provides static access to the current [ITestOutputHelper](https://xunit.net/docs/capturing-output) for use within testing utility methods.

Uses [AsyncLocal](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1) to track state.

toc


## NuGet package

https://nuget.org/packages/XunitLogger/ [![NuGet Status](http://img.shields.io/nuget/v/XunitLogger.svg)](https://www.nuget.org/packages/XunitLogger/)


## ClassBeingTested

snippet: ClassBeingTested.cs


## XunitLoggingBase

`XunitLoggingBase` is an abstract base class for tests. It exposes logging methods for use from unit tests, and handle the flushing of longs in its `Dispose` method. `XunitLoggingBase` is actually a thin wrapper over `XunitLogger`. `XunitLogger`s `Write*` methods can also be use inside a test inheriting from `XunitLoggingBase`.

snippet: TestBaseSample.cs


## XunitLogger

`XunitLogger` provides static access to the logging state for tests. It exposes logging methods for use from unit tests, however registration of [ITestOutputHelper](https://xunit.net/docs/capturing-output) and flushing of logs must be handled explicitly.

snippet: XunitLoggerSample.cs

`XunitLogger` redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write), [Console.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write), and [Debug.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug.write) in its static constructor.

snippet: writeRedirects

These API calls are then routed to the correct xUnit [ITestOutputHelper](https://xunit.net/docs/capturing-output) via a static [AsyncLocal](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1).


## Filters

`XunitLogger.Filters` can be used to filter out unwanted lines:

snippet: FilterSample.cs

Filters are static and shared for all tests.


## Context

For every tests there is a contextual API to perform several operations.

 * `Context.TestOutput`: Access to [ITestOutputHelper](https://xunit.net/docs/capturing-output).
 * `Context.Write` and `Context.WriteLine`: Write to the current log.
 * `Context.LogMessages`: Access to all log message for the current test.
 * Counters: Provide access in predicable and incrementing values for the following types: `Guid`, `Int`, `Long`, `UInt`, and `ULong`.
 * `Context.Test`: Access to the current `ITest`.

There is also access via a static API.

snippet: ContextSample.cs


### Current Test

There is currently no API in xUnit to retrieve information on the current test. See issues [#1359](https://github.com/xunit/xunit/issues/1359), [#416](https://github.com/xunit/xunit/issues/416), and [#398](https://github.com/xunit/xunit/issues/398).

To work around this, this project exposes the current instance of `ITest` via reflection.

Usage:

snippet: CurrentTestSample.cs

Implementation:

snippet: LoggingContext_CurrentTest.cs


### Counters

Provide access to predicable and incrementing values for the following types: `Guid`, `Int`, `Long`, `UInt`, and `ULong`.


#### Non Test Context usage

Counters can also be used outside of the current test context:

snippet: NonTestContextUsage


#### Implementation

snippet: LoggingContext_Counters.cs

snippet: Counters.cs

snippet: GuidCounter.cs

snippet: LongCounter.cs


## Logging Libs

Approaches to routing common logging libraries to Diagnostics.Trace:

 * [Serilog](https://serilog.net/) use [Serilog.Sinks.Trace](https://github.com/serilog/serilog-sinks-trace).
 * [NLog](https://github.com/NLog/NLog) use a [Trace target](https://github.com/NLog/NLog/wiki/Trace-target).


## Icon

[Wolverine](https://thenounproject.com/term/wolverine/18415/) designed by [Mike Rowe](https://thenounproject.com/itsmikerowe/) from [The Noun Project](https://thenounproject.com/).