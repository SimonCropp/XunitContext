# <img src="/src/icon.png" height="30px"> XunitContext

[![Build status](https://ci.appveyor.com/api/projects/status/sdg2ni2jhe2o33le/branch/master?svg=true)](https://ci.appveyor.com/project/SimonCropp/XunitContext)
[![NuGet Status](https://img.shields.io/nuget/v/XunitContext.svg)](https://www.nuget.org/packages/XunitContext/)

Extends [xUnit](https://xunit.net/) to expose extra context and simplify logging.

Redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write), [Debug.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug.write), and [Console.Write and Console.Error.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write) to [ITestOutputHelper](https://xunit.net/docs/capturing-output). Also provides static access to the current [ITestOutputHelper](https://xunit.net/docs/capturing-output) for use within testing utility methods.

Uses [AsyncLocal](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1) to track state.

toc


## NuGet package

https://nuget.org/packages/XunitContext/


## ClassBeingTested

snippet: ClassBeingTested.cs


## XunitContextBase

`XunitContextBase` is an abstract base class for tests. It exposes logging methods for use from unit tests, and handle the flushing of longs in its `Dispose` method. `XunitContextBase` is actually a thin wrapper over `XunitContext`. `XunitContext`s `Write*` methods can also be use inside a test inheriting from `XunitContextBase`.

snippet: TestBaseSample.cs


## Logging

`XunitContext` provides static access to the logging state for tests. It exposes logging methods for use from unit tests, however registration of [ITestOutputHelper](https://xunit.net/docs/capturing-output) and flushing of logs must be handled explicitly.

snippet: XunitLoggerSample.cs

`XunitContext` redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write), [Console.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write), and [Debug.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug.write) in its static constructor.

snippet: writeRedirects

These API calls are then routed to the correct xUnit [ITestOutputHelper](https://xunit.net/docs/capturing-output) via a static [AsyncLocal](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1).


### Logging Libs

Approaches to routing common logging libraries to Diagnostics.Trace:

 * [Serilog](https://serilog.net/) use [Serilog.Sinks.Trace](https://github.com/serilog/serilog-sinks-trace).
 * [NLog](https://github.com/NLog/NLog) use a [Trace target](https://github.com/NLog/NLog/wiki/Trace-target).


## Filters

`XunitContext.Filters` can be used to filter out unwanted lines:

snippet: FilterSample.cs

Filters are static and shared for all tests.


## Context

For every tests there is a contextual API to perform several operations.

 * `Context.TestOutput`: Access to [ITestOutputHelper](https://xunit.net/docs/capturing-output).
 * `Context.Write` and `Context.WriteLine`: Write to the current log.
 * `Context.LogMessages`: Access to all log message for the current test.
 * [Counters](#counters): Provide access in predicable and incrementing values for the following types: `Guid`, `Int`, `Long`, `UInt`, and `ULong`.
 * `Context.Test`: Access to the current `ITest`.
 * `Context.SourceFile`: Access to the file path for the current test.
 * `Context.SourceDirectory`: Access to the directory path for the current test.
 * `Context.SolutionDirectory`: The current solution directory. Obtained by walking up the directory tree from `SourceDirectory`.
 * `Context.TestException`: Access to the exception if the current test has failed. See [Test Failure](test-failure).

snippet: ContextSample.cs

Some members are pushed down to the be accessible directly from `XunitContextBase`:

snippet: ContextPushedDownSample.cs

Context can accessed via a static API:

snippet: ContextStaticSample.cs


### Current Test

There is currently no API in xUnit to retrieve information on the current test. See issues [#1359](https://github.com/xunit/xunit/issues/1359), [#416](https://github.com/xunit/xunit/issues/416), and [#398](https://github.com/xunit/xunit/issues/398).

To work around this, this project exposes the current instance of `ITest` via reflection.

Usage:

snippet: CurrentTestSample.cs

Implementation:

snippet: Context_CurrentTest.cs


### Test Failure

When a test fails it is expressed as an exception. The exception can be viewed by enabling exception capture, and then accessing `Context.TestException`. The `TestException` will be null if the test has passed.

One common case is to perform some logic, based on the existence of the exception, in the `Dispose` of a test.

snippet: TestExceptionSample


### Counters

Provide access to predicable and incrementing values for the following types: `Guid`, `Int`, `Long`, `UInt`, and `ULong`.


#### Non Test Context usage

Counters can also be used outside of the current test context:

snippet: NonTestContextUsage


#### Implementation

snippet: Context_Counters.cs

snippet: Counters.cs

snippet: GuidCounter.cs

snippet: LongCounter.cs


### Base Class

When creating a custom base class for other tests, it is necessary to pass through the source file path to `XunitContextBase` via the constructor.

snippet: XunitContextCustomBase


### Parameters

Provided the parameters passed to the current test when using a `[Theory]`.

Use cases:

 * To derive the [unique test name](#uniquetestname).
 * In extensibility scenarios for example [Verify file naming](https://github.com/SimonCropp/Verify/blob/master/docs/naming.md).

Usage:

snippet: ParametersSample.cs

Implementation:

snippet: Parameters


#### Complex parameters

Only simple types (string, int, DateTime etc) can use the above automated approach. If a complex type is used the following exception will be thrown

include: NoArgumentsDetectedException

To use complex types override the parameter resolution using `XunitContextBase.UseParameters`:

snippet: ComplexParameterSample.cs


### UniqueTestName

Provided a string that uniquely identifies a test case.

Usage:

snippet: UniqueTestNameSample.cs

Implementation:

snippet: UniqueTestName


## Global Setup

Xunit has no way to run code once prior to any tests executing. XUnitContext adds this feature via an attribute.

snippet: GlobalSetup.cs

Multiple setups can be defined as nested classes and classes in namespaces are supported.

Alternatives to this approach:

 * Using a [module initializer ](https://github.com/Fody/ModuleInit).
 * Having a single base class that all tests inherit from, and place any configuration code in the static constructor of that type.


## Release Notes

See [closed milestones](../../milestones?state=closed).


## Icon

[Wolverine](https://thenounproject.com/term/wolverine/18415/) designed by [Mike Rowe](https://thenounproject.com/itsmikerowe/) from [The Noun Project](https://thenounproject.com/).