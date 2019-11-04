using Xunit;
using Xunit.Abstractions;

public class ContextStaticSample :
    XunitLoggingBase
{
    [Fact]
    public void StaticUsage()
    {
        var currentGuid = XunitLogging.Context.CurrentGuid;

        var nextGuid = XunitLogging.Context.NextGuid();

        XunitLogging.Context.WriteLine("Some message");

        var currentLogMessages = XunitLogging.Context.LogMessages;

        var testOutputHelper = XunitLogging.Context.TestOutput;

        var currentTest = XunitLogging.Context.Test;

        var sourceFile = XunitLogging.Context.SourceFile;

        var sourceDirectory = XunitLogging.Context.SourceDirectory;

        var currentTestException = XunitLogging.Context.TestException;
    }

    public ContextStaticSample(ITestOutputHelper output) :
        base(output)
    {
    }
}