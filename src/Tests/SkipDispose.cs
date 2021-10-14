using System;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

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
        return Verifier.Verify(Logs);
    }

    [Fact]
    public Task Write_after_dispose_should_throw()
    {
        base.Dispose();
        var exception = Assert.Throws<Exception>(WriteLine);

        return Verifier.Verify(exception);
    }

    public SkipDispose(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }

    public override void Dispose()
    {
    }
}