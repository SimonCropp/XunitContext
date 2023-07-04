namespace Xunit;

public class ContextFixture : IDisposable
{
    Context? context;

    public Context Start(ITestOutputHelper outputHelper, [CallerFilePath] string sourceFile = "") =>
        context = XunitContext.Register(outputHelper, sourceFile);

    public void Dispose() =>
        context?.Flush();
}