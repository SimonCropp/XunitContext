#region TestExceptionSample

// ReSharper disable UnusedVariable
public static class GlobalSetup
{
    [ModuleInitializer]
    public static void Setup() =>
        XunitContext.EnableExceptionCapture();
}

public class TestExceptionSample(ITestOutputHelper output) :
    XunitContextBase(output)
{
    [Fact(Skip = "Will fail")]
    public void Usage() =>
        //This tests will fail
        Assert.False(true);

    public override void Dispose()
    {
        var theExceptionThrownByTest = Context.TestException;
        var testDisplayName = Context.Test.DisplayName;
        var testCase = Context.Test.TestCase;
        base.Dispose();
    }
}

#endregion