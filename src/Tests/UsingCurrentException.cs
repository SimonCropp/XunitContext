using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

[UsesVerify]
public class UsingCurrentException :
    XunitContextBase
{
    static UsingCurrentException()
    {
        XunitContext.EnableExceptionCapture();
    }

    [Fact]
    [Trait("Category", "Integration")]
    public void Fails()
    {
        Assert.True(false);
    }

    [Fact]
    public void Passes()
    {
    }

    public UsingCurrentException(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }

    public override void Dispose()
    {
        var contextTestException = Context.TestException;
        var methodName = Context.Test.TestCase.TestMethod.Method.Name;
        if (methodName == "Passes")
        {
            Assert.Null(contextTestException);
        }
        if (methodName == "Fails")
        {
            Assert.NotNull(contextTestException);
        }
        base.Dispose();
    }
}