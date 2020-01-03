using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class SkipDispose :
    XunitContextBase
{
    [Fact]
    public void Dispose_should_flush()
    {
        Write("part1");
        Write(" part2");
        base.Dispose();
        ObjectApprover.Verify(Logs);
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