namespace Xunit.Fixture;

public class ContextFixture : IDisposable
{
    public Context? Context { get; private set; }

    public Context Start(ITestOutputHelper h, [CallerFilePath] string sourceFile = "")
    {
        Context = XunitContext.Register(h, sourceFile);
        return Context;
    }

    public void Dispose() =>
        Context?.Flush();
}