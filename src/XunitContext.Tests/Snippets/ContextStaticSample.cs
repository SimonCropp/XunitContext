// ReSharper disable UnusedVariable

public class ContextStaticSample(ITestOutputHelper output) :
    XunitContextBase(output)
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
}