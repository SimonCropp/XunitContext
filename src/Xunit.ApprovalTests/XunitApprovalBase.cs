using System.Runtime.CompilerServices;
using Xunit.Abstractions;

public abstract class XunitApprovalBase :
    XunitContextBase
{
    protected XunitApprovalBase(
        ITestOutputHelper output,
        [CallerFilePath] string sourceFile = "") :
        base(output, sourceFile)
    {
     //   Approvals.SetTestData(Context.TestType, Context.MethodInfo);
    }

    //public override void Dispose()
    //{
    //    base.Dispose();
    //    Approvals.ClearTestData();
    //}
}