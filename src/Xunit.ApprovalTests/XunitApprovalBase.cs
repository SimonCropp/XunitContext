using Xunit.Abstractions;

public abstract class XunitApprovalBase :
    XunitLoggingBase
{
    protected XunitApprovalBase(ITestOutputHelper output) :
        base(output)
    {
    }
}