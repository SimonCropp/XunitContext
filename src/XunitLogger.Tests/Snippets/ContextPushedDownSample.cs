using Xunit;
using Xunit.Abstractions;

public class ContextPushedDownSample  :
    XunitLoggingBase
{
    [Fact]
    public void Usage()
    {
        WriteLine("Some message");

        var currentLogMessages = Logs;

        var testOutputHelper = Output;

        var sourceFile = SourceFile;

        var sourceDirectory = SourceDirectory;
    }

    public ContextPushedDownSample(ITestOutputHelper output) :
        base(output)
    {
    }
}