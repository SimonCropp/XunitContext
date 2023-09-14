// ReSharper disable UnusedVariable
public class ContextSample(ITestOutputHelper output) :
    XunitContextBase(output)
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
}