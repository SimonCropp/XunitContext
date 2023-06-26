namespace Xunit.Fixture;

public class ContextFixture : IDisposable
{
    private Context? context;

    public Context Start(ITestOutputHelper h, [CallerFilePath] string sourceFile = "")
    {
        context = XunitContext.Register(h, sourceFile);
        return context;
    }

    public void Dispose() =>
        context?.Flush();
}