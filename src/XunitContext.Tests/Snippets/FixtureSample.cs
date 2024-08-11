public class FixtureSample(ITestOutputHelper helper, ContextFixture ctxFixture) :
    IContextFixture
{
    Context context = ctxFixture.Start(helper);

    [Fact]
    public void Usage()
    {
        Console.WriteLine("From Test");
        Assert.Contains("From Test", context.LogMessages);
    }
}