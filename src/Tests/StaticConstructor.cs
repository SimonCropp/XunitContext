[UsesVerify]
public class StaticConstructor
{
    static StaticConstructor()
    {
        var type = typeof(XunitContext);
        Console.Write("a");
        Console.WriteLine("Foo");
    }

    [Fact]
    public Task VerifyLogs()
    {
        Assert.EndsWith("StaticConstructor.cs", XunitContext.Context.SourceFile);
        var logs = XunitContext.Flush(false);
        return Verify(logs);
    }

    public StaticConstructor(ITestOutputHelper testOutput)
    {
        XunitContext.Register(testOutput);
    }
}