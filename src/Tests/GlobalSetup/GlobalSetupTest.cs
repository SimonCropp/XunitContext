using Xunit;
using Xunit.Abstractions;

public class GlobalSetupTest :
    XunitContextBase
{
    [Fact]
    public void VerifyCalled()
    {
        Assert.True(GlobalSetup.Called);
        Assert.True(InNamespace.GlobalSetup.Called);
        Assert.True(Nested.GlobalSetup.Called);
    }

    public GlobalSetupTest(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}