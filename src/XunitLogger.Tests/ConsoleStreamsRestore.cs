using System;
using Xunit;
using Xunit.Abstractions;

public class ConsoleStreamsRestore : XunitLoggingBase
{
    public ConsoleStreamsRestore(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void RestoresConsoleOut()
    {
        const string log1 = "should be visible in test output";
        const string log2 = "should not be visible in test output";
        
        Console.WriteLine(log1);
        
        XunitLogging.RestoreConsoleStreams();
        
        Console.WriteLine(log2);
        
        var logs = XunitLogging.Flush();
        
        var storedLog = Assert.Single(logs);
        Assert.Equal(log1, storedLog);
    }
}