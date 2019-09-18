using System.Runtime.CompilerServices;
using Xunit.Abstractions;

public abstract class XunitApprovalBase :
    XunitLoggingBase
{
    protected XunitApprovalBase(
        ITestOutputHelper output,
        [CallerFilePath] string sourceFile = "") :
        base(output, sourceFile)
    {
    }
}