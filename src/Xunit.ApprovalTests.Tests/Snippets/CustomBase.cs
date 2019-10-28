using Xunit.Abstractions;

#region XunitApprovalCustomBase
public class CustomBase :
    XunitApprovalBase
{
    public CustomBase(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}
#endregion