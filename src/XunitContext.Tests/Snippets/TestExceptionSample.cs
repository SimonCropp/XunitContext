using Xunit;
using Xunit.Abstractions;

[Trait("Category", "Integration")]

#region TestExceptionSample

public class TestExceptionSample :
    XunitContextBase
{
    static TestExceptionSample()
    {
        //Should be called once at appdomain startup
        XunitContext.EnableExceptionCapture();
    }

    [Fact]
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
        var testDisplayName = Context.Test.DisplayName;
        var testCase = Context.Test.TestCase;
        base.Dispose();
    }
}
#endregion