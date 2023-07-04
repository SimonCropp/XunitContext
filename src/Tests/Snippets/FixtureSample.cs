public class FixtureSample :
    IContextFixture
{
    Context context;

    public FixtureSample(ITestOutputHelper helper, ContextFixture ctxFixture) =>
        context = ctxFixture.Start(helper);

    [Fact]
    public void Usage()
    {
        Console.WriteLine("From Test");
        Assert.Contains("From Test", context.LogMessages);
    }
}