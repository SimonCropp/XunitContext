using Xunit;
using Xunit.Abstractions;

public class TestBaseSample :
    XunitLoggingBase
{
    [Fact]
    public void Write_lines()
    {
        WriteLine("From Test");
        ClassBeingTested.Method();

        var logs = XunitLogger.Logs;

        Assert.Contains("From Test", logs);
        Assert.Contains("From Trace", logs);
        Assert.Contains("From Console", logs);
    }

    public TestBaseSample(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}