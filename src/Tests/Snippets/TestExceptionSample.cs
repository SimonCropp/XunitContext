using Xunit;
using Xunit.Abstractions;

public class TestExceptionSample :
    XunitLoggingBase
{
    static TestExceptionSample()
    {
        //Called once at startup
        XunitLogging.EnableExceptionCapture();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void Usage()
    {
        //This tests will fail
        Assert.False(true);
    }

    public TestExceptionSample(ITestOutputHelper output) :
        base(output)
    {
    }

    public override void Dispose()
    {
        var theExceptionThrownByTest = Context.TestException;

        base.Dispose();
    }
}