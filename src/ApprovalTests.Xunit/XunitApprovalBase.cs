using System.Runtime.CompilerServices;
using Xunit.Abstractions;

public abstract class XunitApprovalBase :
    XunitLoggingBase
{
    protected XunitApprovalBase(
        ITestOutputHelper output,
        [CallerFilePath] string sourceFilePath = "") :
        base(output, sourceFilePath)
    {
    }
}