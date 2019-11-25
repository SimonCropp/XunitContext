using Xunit;
using Xunit.Abstractions;

public class GlobalSetupTest :
    XunitContextBase
{
    [Fact]
    public void VerifyCalled()
    {
        Assert.True(XunitGlobalSetup.Called);
        Assert.True(InNamespace.XunitGlobalSetup.Called);
        Assert.True(Nested.XunitGlobalSetup.Called);
    }

    public GlobalSetupTest(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}