using System.Runtime.CompilerServices;
using Xunit.Abstractions;

#region XunitApprovalCustomBase
public class CustomBase :
    XunitApprovalBase
{
    public CustomBase(
        ITestOutputHelper testOutput,
        [CallerFilePath] string sourceFile = "") :
        base(testOutput, sourceFile)
    {
    }
}
#endregion