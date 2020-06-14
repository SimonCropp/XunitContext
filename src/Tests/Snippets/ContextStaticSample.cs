using Xunit;
using Xunit.Abstractions;

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