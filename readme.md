# <img src="/src/icon.png" height="30px"> XunitContext

[![Build status](https://ci.appveyor.com/api/projects/status/sdg2ni2jhe2o33le/branch/main?svg=true)](https://ci.appveyor.com/project/SimonCropp/XunitContext)
[![NuGet Status](https://img.shields.io/nuget/v/XunitContext.svg)](https://www.nuget.org/packages/XunitContext/)

Extends [xUnit](https://xunit.net/) to expose extra context and simplify logging.

Redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write), [Debug.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug.write), and [Console.Write and Console.Error.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write) to [ITestOutputHelper](https://xunit.net/docs/capturing-output). Also provides static access to the current [ITestOutputHelper](https://xunit.net/docs/capturing-output) for use within testing utility methods.

Uses [AsyncLocal](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1) to track state.


## NuGet package

https://nuget.org/packages/XunitContext/


## ClassBeingTested

<!-- snippet: ClassBeingTested.cs -->
<a id='snippet-ClassBeingTested.cs'></a>
```cs
static class ClassBeingTested
{
    public static void Method()
    {
        Trace.WriteLine("From Trace");
        Console.WriteLine("From Console");
        Debug.WriteLine("From Debug");
        Console.Error.WriteLine("From Console Error");
    }
}
```
<sup><a href='/src/Tests/Snippets/ClassBeingTested.cs#L1-L10' title='Snippet source file'>snippet source</a> | <a href='#snippet-ClassBeingTested.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## XunitContextBase

`XunitContextBase` is an abstract base class for tests. It exposes logging methods for use from unit tests, and handle the flushing of logs in its `Dispose` method. `XunitContextBase` is actually a thin wrapper over `XunitContext`. `XunitContext`s `Write*` methods can also be use inside a test inheriting from `XunitContextBase`.

<!-- snippet: TestBaseSample.cs -->
<a id='snippet-TestBaseSample.cs'></a>
```cs
public class TestBaseSample  :
    XunitContextBase
{
    [Fact]
    public void Write_lines()
    {
        WriteLine("From Test");
        ClassBeingTested.Method();

        var logs = XunitContext.Logs;

        Assert.Contains("From Test", logs);
        Assert.Contains("From Trace", logs);
        Assert.Contains("From Debug", logs);
        Assert.Contains("From Console", logs);
        Assert.Contains("From Console Error", logs);
    }

    public TestBaseSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/TestBaseSample.cs#L1-L23' title='Snippet source file'>snippet source</a> | <a href='#snippet-TestBaseSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## xunit Fixture

In addition to `XunitContextBase` class approach, one is also possible to use  `IContextFixture` to gain access to `XunitContext` :

<!-- snippet: FixtureSample.cs -->
<a id='snippet-FixtureSample.cs'></a>
```cs
public class FixtureSample :
    IContextFixture
{
    Context context;

    public FixtureSample(ITestOutputHelper helper, ContextFixture ctxFixture) =>
        context = ctxFixture.Start(helper);

    [Fact]
    public void Usage()
    {
        Console.WriteLine("From Test");
        Assert.Contains("From Test", context.LogMessages);
    }
}
```
<sup><a href='/src/Tests/Snippets/FixtureSample.cs#L1-L15' title='Snippet source file'>snippet source</a> | <a href='#snippet-FixtureSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Logging

`XunitContext` provides static access to the logging state for tests. It exposes logging methods for use from unit tests, however registration of [ITestOutputHelper](https://xunit.net/docs/capturing-output) and flushing of logs must be handled explicitly.

<!-- snippet: XunitLoggerSample.cs -->
<a id='snippet-XunitLoggerSample.cs'></a>
```cs
public class XunitLoggerSample :
    IDisposable
{
    [Fact]
    public void Usage()
    {
        XunitContext.WriteLine("From Test");

        ClassBeingTested.Method();

        var logs = XunitContext.Logs;

        Assert.Contains("From Test", logs);
        Assert.Contains("From Trace", logs);
        Assert.Contains("From Debug", logs);
        Assert.Contains("From Console", logs);
        Assert.Contains("From Console Error", logs);
    }

    public XunitLoggerSample(ITestOutputHelper testOutput) =>
        XunitContext.Register(testOutput);

    public void Dispose() =>
        XunitContext.Flush();
}
```
<sup><a href='/src/Tests/Snippets/XunitLoggerSample.cs#L1-L25' title='Snippet source file'>snippet source</a> | <a href='#snippet-XunitLoggerSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

`XunitContext` redirects [Trace.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.trace.write), [Console.Write](https://docs.microsoft.com/en-us/dotnet/api/system.console.write), and [Debug.Write](https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.debug.write) in its static constructor.

<!-- snippet: writeRedirects -->
<a id='snippet-writeredirects'></a>
```cs
Trace.Listeners.Clear();
Trace.Listeners.Add(new TraceListener());
#if (NETSTANDARD)
DebugPoker.Overwrite(
    text =>
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        if (text.EndsWith(Environment.NewLine))
        {
            WriteLine(text.TrimTrailingNewline());
            return;
        }

        Write(text);
    });
#else
Debug.Listeners.Clear();
Debug.Listeners.Add(new TraceListener());
#endif
TestWriter writer = new();
Console.SetOut(writer);
Console.SetError(writer);
```
<sup><a href='/src/XunitContext/XunitContext.cs#L43-L72' title='Snippet source file'>snippet source</a> | <a href='#snippet-writeredirects' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

These API calls are then routed to the correct xUnit [ITestOutputHelper](https://xunit.net/docs/capturing-output) via a static [AsyncLocal](https://docs.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1).


### Logging Libs

Approaches to routing common logging libraries to Diagnostics.Trace:

 * [Serilog](https://serilog.net/) use [Serilog.Sinks.Trace](https://github.com/serilog/serilog-sinks-trace).
 * [NLog](https://github.com/NLog/NLog) use a [Trace target](https://github.com/NLog/NLog/wiki/Trace-target).


## Filters

`XunitContext.Filters` can be used to filter out unwanted lines:

<!-- snippet: FilterSample.cs -->
<a id='snippet-FilterSample.cs'></a>
```cs
public class FilterSample :
    XunitContextBase
{
    static FilterSample() =>
        Filters.Add(_ => _ != null && !_.Contains("ignored"));

    [Fact]
    public void Write_lines()
    {
        WriteLine("first");
        WriteLine("with ignored string");
        WriteLine("last");
        var logs = XunitContext.Logs;

        Assert.Contains("first", logs);
        Assert.DoesNotContain("with ignored string", logs);
        Assert.Contains("last", logs);
    }

    public FilterSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/FilterSample.cs#L1-L24' title='Snippet source file'>snippet source</a> | <a href='#snippet-FilterSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

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

<!-- snippet: ContextSample.cs -->
<a id='snippet-ContextSample.cs'></a>
```cs
public class ContextSample  :
    XunitContextBase
{
    [Fact]
    public void Usage()
    {
        Context.WriteLine("Some message");

        var currentLogMessages = Context.LogMessages;

        var testOutputHelper = Context.TestOutput;

        var currentTest = Context.Test;

        var sourceFile = Context.SourceFile;

        var sourceDirectory = Context.SourceDirectory;

        var solutionDirectory = Context.SolutionDirectory;

        var currentTestException = Context.TestException;
    }

    public ContextSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/ContextSample.cs#L1-L28' title='Snippet source file'>snippet source</a> | <a href='#snippet-ContextSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Some members are pushed down to the be accessible directly from `XunitContextBase`:

<!-- snippet: ContextPushedDownSample.cs -->
<a id='snippet-ContextPushedDownSample.cs'></a>
```cs
public class ContextPushedDownSample  :
    XunitContextBase
{
    [Fact]
    public void Usage()
    {
        WriteLine("Some message");

        var currentLogMessages = Logs;

        var testOutputHelper = Output;

        var sourceFile = SourceFile;

        var sourceDirectory = SourceDirectory;

        var solutionDirectory = SolutionDirectory;

        var currentTestException = TestException;
    }

    public ContextPushedDownSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/ContextPushedDownSample.cs#L1-L26' title='Snippet source file'>snippet source</a> | <a href='#snippet-ContextPushedDownSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Context can accessed via a static API:

<!-- snippet: ContextStaticSample.cs -->
<a id='snippet-ContextStaticSample.cs'></a>
```cs
public class ContextStaticSample :
    XunitContextBase
{
    [Fact]
    public void StaticUsage()
    {
        XunitContext.Context.WriteLine("Some message");

        var currentLogMessages = XunitContext.Context.LogMessages;

        var testOutputHelper = XunitContext.Context.TestOutput;

        var currentTest = XunitContext.Context.Test;

        var sourceFile = XunitContext.Context.SourceFile;

        var sourceDirectory = XunitContext.Context.SourceDirectory;

        var solutionDirectory = XunitContext.Context.SolutionDirectory;

        var currentTestException = XunitContext.Context.TestException;
    }

    public ContextStaticSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/ContextStaticSample.cs#L1-L28' title='Snippet source file'>snippet source</a> | <a href='#snippet-ContextStaticSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Current Test

There is currently no API in xUnit to retrieve information on the current test. See issues [#1359](https://github.com/xunit/xunit/issues/1359), [#416](https://github.com/xunit/xunit/issues/416), and [#398](https://github.com/xunit/xunit/issues/398).

To work around this, this project exposes the current instance of `ITest` via reflection.

Usage:

<!-- snippet: CurrentTestSample.cs -->
<a id='snippet-CurrentTestSample.cs'></a>
```cs
public class CurrentTestSample :
    XunitContextBase
{
    [Fact]
    public void Usage()
    {
        var currentTest = Context.Test;
        // DisplayName will be 'TestNameSample.Usage'
        var displayName = currentTest.DisplayName;
    }

    [Fact]
    public void StaticUsage()
    {
        var currentTest = XunitContext.Context.Test;
        // DisplayName will be 'TestNameSample.StaticUsage'
        var displayName = currentTest.DisplayName;
    }

    public CurrentTestSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/CurrentTestSample.cs#L1-L24' title='Snippet source file'>snippet source</a> | <a href='#snippet-CurrentTestSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Implementation:

<!-- snippet: Context_CurrentTest.cs -->
<a id='snippet-Context_CurrentTest.cs'></a>
```cs
using Xunit.Sdk;

namespace Xunit;

public partial class Context
{
    ITest? test;

    static FieldInfo? cachedTestMember;

    public ITest Test
    {
        get
        {
            InitTest();

            return test!;
        }
    }

    MethodInfo? methodInfo;
    public MethodInfo MethodInfo
    {
        get
        {
            InitTest();
            return methodInfo!;
        }
    }

    Type? testType;
    public Type TestType
    {
        get
        {
            InitTest();
            return testType!;
        }
    }

    void InitTest()
    {
        if (test != null)
        {
            return;
        }
        test = (ITest) GetTestMethod().GetValue(TestOutput)!;
        var method = (ReflectionMethodInfo) test.TestCase.TestMethod.Method;
        var type = (ReflectionTypeInfo) test.TestCase.TestMethod.TestClass.Class;
        methodInfo = method.MethodInfo;
        testType = type.Type;
    }

    public static string MissingTestOutput = "ITestOutputHelper has not been set. It is possible that the call to `XunitContext.Register()` is missing, or the current test does not inherit from `XunitContextBase`.";

    FieldInfo GetTestMethod()
    {
        if (TestOutput == null)
        {
            throw new(MissingTestOutput);
        }

        if (cachedTestMember != null)
        {
            return cachedTestMember;
        }

        var testOutputType = TestOutput.GetType();
        cachedTestMember = testOutputType.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        if (cachedTestMember == null)
        {
            throw new($"Unable to find 'test' field on {testOutputType.FullName}");
        }

        return cachedTestMember;
    }
}
```
<sup><a href='/src/XunitContext/Context_CurrentTest.cs#L1-L77' title='Snippet source file'>snippet source</a> | <a href='#snippet-Context_CurrentTest.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Test Failure

When a test fails it is expressed as an exception. The exception can be viewed by enabling exception capture, and then accessing `Context.TestException`. The `TestException` will be null if the test has passed.

One common case is to perform some logic, based on the existence of the exception, in the `Dispose` of a test.

<!-- snippet: TestExceptionSample -->
<a id='snippet-testexceptionsample'></a>
```cs
public static class GlobalSetup
{
    [ModuleInitializer]
    public static void Setup() =>
        XunitContext.EnableExceptionCapture();
}

[Trait("Category", "Integration")]
public class TestExceptionSample :
    XunitContextBase
{
    [Fact]
    public void Usage() =>
        //This tests will fail
        Assert.False(true);

    public TestExceptionSample(ITestOutputHelper output) :
        base(output)
    {
    }

    public override void Dispose()
    {
        var theExceptionThrownByTest = Context.TestException;
        var testDisplayName = Context.Test.DisplayName;
        var testCase = Context.Test.TestCase;
        base.Dispose();
    }
}
```
<sup><a href='/src/Tests/Snippets/TestExceptionSample.cs#L1-L32' title='Snippet source file'>snippet source</a> | <a href='#snippet-testexceptionsample' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Base Class

When creating a custom base class for other tests, it is necessary to pass through the source file path to `XunitContextBase` via the constructor.

<!-- snippet: XunitContextCustomBase -->
<a id='snippet-xunitcontextcustombase'></a>
```cs
public class CustomBase :
    XunitContextBase
{
    public CustomBase(
        ITestOutputHelper testOutput,
        [CallerFilePath] string sourceFile = "") :
        base(testOutput, sourceFile)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/CustomBase.cs#L1-L12' title='Snippet source file'>snippet source</a> | <a href='#snippet-xunitcontextcustombase' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Parameters

Provided the parameters passed to the current test when using a `[Theory]`.

Use cases:

 * To derive the [unique test name](#uniquetestname).
 * In extensibility scenarios for example [Verify file naming](https://github.com/SimonCropp/Verify/blob/master/docs/naming.md).

Usage:

<!-- snippet: ParametersSample.cs -->
<a id='snippet-ParametersSample.cs'></a>
```cs
public class ParametersSample :
    XunitContextBase
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void Usage(string arg)
    {
        var parameter = Context.Parameters.Single();
        var parameterInfo = parameter.Info;
        Assert.Equal("arg", parameterInfo.Name);
        Assert.Equal(arg, parameter.Value);
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[] {"Value1"};
        yield return new object[] {"Value2"};
    }

    public ParametersSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/ParametersSample.cs#L1-L25' title='Snippet source file'>snippet source</a> | <a href='#snippet-ParametersSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Implementation:

<!-- snippet: Parameters -->
<a id='snippet-parameters'></a>
```cs
static List<Parameter> GetParameters(ITestCase testCase) =>
    GetParameters(testCase, testCase.TestMethodArguments);

static List<Parameter> GetParameters(ITestCase testCase, object[] arguments)
{
    var method = testCase.TestMethod;
    var infos = method.Method.GetParameters().ToList();
    if (arguments == null || !arguments.Any())
    {
        if (infos.Count == 0)
        {
            return empty;
        }

        throw NewNoArgumentsDetectedException();
    }

    List<Parameter> items = new();

    for (var index = 0; index < infos.Count; index++)
    {
        items.Add(new(infos[index], arguments[index]));
    }

    return items;
}
```
<sup><a href='/src/XunitContext/Context_Parameters.cs#L20-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-parameters' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


#### Complex parameters

Only core types (string, int, DateTime etc) can use the above automated approach. If a complex type is used the following exception will be thrown

 <!-- include: NoArgumentsDetectedException. path: /src/Tests/NoArgumentsDetectedException.include.md -->
> No arguments detected for method with parameters.
> This is most likely caused by using a parameter that Xunit cannot serialize.
> Instead pass in a simple type as a parameter and construct the complex object inside the test.
> Alternatively; override the current parameters using `UseParameters()` via the current test base class, or via `XunitContext.Current.UseParameters()`.
 <!-- endInclude -->

To use complex types override the parameter resolution using `XunitContextBase.UseParameters`:

<!-- snippet: ComplexParameterSample.cs -->
<a id='snippet-ComplexParameterSample.cs'></a>
```cs
public class ComplexParameterSample :
    XunitContextBase
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void UseComplexMemberData(ComplexClass arg)
    {
        UseParameters(arg);
        var parameter = Context.Parameters.Single();
        var parameterInfo = parameter.Info;
        Assert.Equal("arg", parameterInfo.Name);
        Assert.Equal(arg, parameter.Value);
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[] {new ComplexClass("Value1")};
        yield return new object[] {new ComplexClass("Value2")};
    }

    public ComplexParameterSample(ITestOutputHelper output) :
        base(output)
    {
    }

    public class ComplexClass
    {
        public string Value { get; }

        public ComplexClass(string value) =>
            Value = value;
    }
}
```
<sup><a href='/src/Tests/Snippets/ComplexParameterSample.cs#L1-L33' title='Snippet source file'>snippet source</a> | <a href='#snippet-ComplexParameterSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### UniqueTestName

Provided a string that uniquely identifies a test case.

Usage:

<!-- snippet: UniqueTestNameSample.cs -->
<a id='snippet-UniqueTestNameSample.cs'></a>
```cs
public class UniqueTestNameSample :
    XunitContextBase
{
    [Fact]
    public void Usage()
    {
        var testName = Context.UniqueTestName;

        Context.WriteLine(testName);
    }

    public UniqueTestNameSample(ITestOutputHelper output) :
        base(output)
    {
    }
}
```
<sup><a href='/src/Tests/Snippets/UniqueTestNameSample.cs#L1-L16' title='Snippet source file'>snippet source</a> | <a href='#snippet-UniqueTestNameSample.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Implementation:

<!-- snippet: UniqueTestName -->
<a id='snippet-uniquetestname'></a>
```cs
string GetUniqueTestName(ITestCase testCase)
{
    var method = testCase.TestMethod;
    var name = $"{method.TestClass.Class.ClassName()}.{method.Method.Name}";
    if (!Parameters.Any())
    {
        return name;
    }

    StringBuilder builder = new($"{name}_");
    foreach (var parameter in Parameters)
    {
        builder.Append($"{parameter.Info.Name}=");
        builder.Append(string.Join(",", SplitParams(parameter.Value)));
        builder.Append('_');
    }

    builder.Length -= 1;

    return builder.ToString();
}

static IEnumerable<string> SplitParams(object? parameter)
{
    if (parameter == null)
    {
        yield return "null";
        yield break;
    }

    if (parameter is string stringValue)
    {
        yield return stringValue;
        yield break;
    }

    if (parameter is IEnumerable enumerable)
    {
        foreach (var item in enumerable)
        {
            foreach (var sub in SplitParams(item))
            {
                yield return sub;
            }
        }

        yield break;
    }

    yield return parameter.ToString();
}
```
<sup><a href='/src/XunitContext/Context_TestName.cs#L24-L76' title='Snippet source file'>snippet source</a> | <a href='#snippet-uniquetestname' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Global Setup

Xunit has no way to run code once before any tests executing. So use one of the following:

 * [C# 9 Module Initializer](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-9.0/module-initializers).
 * [Fody Module Initializer](https://github.com/Fody/ModuleInit).
 * Having a single base class that all tests inherit from, and place any configuration code in the static constructor of that type.



## Icon

[Wolverine](https://thenounproject.com/term/wolverine/18415/) designed by [Mike Rowe](https://thenounproject.com/itsmikerowe/) from [The Noun Project](https://thenounproject.com/).
