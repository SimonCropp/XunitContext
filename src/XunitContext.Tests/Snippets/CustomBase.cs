using System.Runtime.CompilerServices;
using Xunit;
using Xunit.Abstractions;

#region XunitContextCustomBase
public class CustomBase :
    XunitContextBase
{
    public CustomBase(
        ITestOutputHelper testOutput,
        [CallerFilePath] string sourceFile = "") :
        base(testOutput, sourceFile)
    {
    }
}
#endregion