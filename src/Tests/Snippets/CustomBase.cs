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