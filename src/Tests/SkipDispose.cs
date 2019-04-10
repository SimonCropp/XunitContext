using System;
using ObjectApproval;
using Xunit;
using Xunit.Abstractions;

public class SkipDispose :
    XunitLoggingBase
{
    [Fact]
    public void Dispose_should_flush()
    {
        Write("part1");
        Write(" part2");
        base.Dispose();
        ObjectApprover.VerifyWithJson(Logs);
    }

    [Fact]
    public void Write_after_dispose_should_throw()
    {
        base.Dispose();
        var exception = Assert.Throws<Exception>(() => WriteLine());
        ObjectApprover.VerifyWithJson(exception);
    }

    public SkipDispose(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }

    public override void Dispose()
    {
    }
}