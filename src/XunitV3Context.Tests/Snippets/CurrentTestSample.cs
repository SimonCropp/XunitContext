// ReSharper disable UnusedVariable

public class CurrentTestSample(ITestOutputHelper output) :
    XunitContextBase(output)
{
    [Fact]
    public void Usage()
    {
        var currentTest = Context.Test;
        // DisplayName will be 'CurrentTestSample.Usage'
        var displayName = currentTest.TestDisplayName;
    }

    [Fact]
    public void StaticUsage()
    {
        var currentTest = XunitContext.Context.Test;
        // DisplayName will be 'CurrentTestSample.StaticUsage'
        var displayName = currentTest.TestDisplayName;
    }
}