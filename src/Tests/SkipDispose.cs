[UsesVerifyAttribute]
public class SkipDispose :
    XunitContextBase
{
    [Fact]
    public Task Dispose_should_flush()
    {
        Write("part1");
        Write(" part2");
        base.Dispose();
        return Verify(Logs);
    }

    [Fact]
    public Task Write_after_dispose_should_throw()
    {
        base.Dispose();
        var exception = Assert.Throws<Exception>(WriteLine);

        return Verify(exception);
    }

    public SkipDispose(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }

    public override void Dispose()
    {
    }
}