using MarkdownSnippets;
using Xunit;
using Xunit.Abstractions;

public class DocoUpdater:
    XunitLoggingBase
{
    [Fact]
    public void Run()
    {
        DirectoryMarkdownProcessor.RunForFilePath();
    }

    public DocoUpdater(ITestOutputHelper output) :
        base(output)
    {
    }
}