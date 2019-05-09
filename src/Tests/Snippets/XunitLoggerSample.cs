using System;
using Xunit;
using Xunit.Abstractions;

public class XunitLoggerSample :
    IDisposable
{
    [Fact]
    public void Usage()
    {
        XunitLogging.WriteLine("From Test");

        ClassBeingTested.Method();

        var logs = XunitLogging.Logs;

        Assert.Contains("From Test", logs);
        Assert.Contains("From Trace", logs);
        Assert.Contains("From Debug", logs);
        Assert.Contains("From Console", logs);
        Assert.Contains("From Console Error", logs);
    }

    public XunitLoggerSample(ITestOutputHelper testOutput)
    {
        XunitLogging.Register(testOutput);
    }

    public void Dispose()
    {
        XunitLogging.Flush();
    }
}