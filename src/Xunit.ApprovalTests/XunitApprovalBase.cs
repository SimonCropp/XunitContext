using System.Runtime.CompilerServices;
using Xunit.Abstractions;
using Xunit.ApprovalTests;

public abstract class XunitApprovalBase :
    XunitLoggingBase
{
    protected XunitApprovalBase(
        ITestOutputHelper output,
        [CallerFilePath] string sourceFile = "") :
        base(output, sourceFile)
    {
        Approvals.SetTestData(Context.TestType, Context.MethodInfo);
    }

    public override void Dispose()
    {
        base.Dispose();
        Approvals.ClearTestData();
    }
}