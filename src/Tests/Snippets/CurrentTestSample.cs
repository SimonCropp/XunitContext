// ReSharper disable UnusedVariable

public class CurrentTestSample(ITestOutputHelper output) :
    XunitContextBase(output)
{
    [Fact]
    public void Usage()
    {
        var currentTest = Context.Test;
        // DisplayName will be 'TestNameSample.Usage'
        var displayName = currentTest.DisplayName;
    }

    [Fact]
    public void StaticUsage()
    {
        var currentTest = XunitContext.Context.Test;
        // DisplayName will be 'TestNameSample.StaticUsage'
        var displayName = currentTest.DisplayName;
    }
}