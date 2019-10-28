using Xunit.Abstractions;

#region XunitLoggingCustomBase
public class CustomBase :
    XunitLoggingBase
{
    protected CustomBase(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}
#endregion