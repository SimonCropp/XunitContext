#region TestExceptionSample

// ReSharper disable UnusedVariable
public static class GlobalSetup
{
    [ModuleInitializer]
    public static void Setup() =>
        XunitContext.EnableExceptionCapture();
}

[Trait("Category", "Integration")]
public class TestExceptionSample(ITestOutputHelper output) :
    XunitContextBase(output)
{
    [Fact]
    public void Usage() =>
        //This tests will fail
        Assert.False(true);

    public override void Dispose()
    {
        var theExceptionThrownByTest = Context.TestException;
        var testDisplayName = Context.Test.TestDisplayName;
        var testCase = Context.Test.TestCase;
        base.Dispose();
    }
}

#endregion