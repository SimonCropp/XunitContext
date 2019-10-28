using System.Runtime.CompilerServices;
using Xunit.Abstractions;

#region XunitLoggingCustomBase
public class CustomBase :
    XunitLoggingBase
{
    public CustomBase(
        ITestOutputHelper testOutput,
        [CallerFilePath] string sourceFile = "") :
        base(testOutput, sourceFile)
    {
    }
}
#endregion