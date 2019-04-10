using System;
using ObjectApproval;
using Xunit;
using Xunit.Abstractions;

public class UsingStatic
{
    [Fact]
    public void Write_lines()
    {
        XunitLogger.Write("part1");
        XunitLogger.Write(" part2");
        XunitLogger.WriteLine();
        XunitLogger.WriteLine("part3");
        ObjectApprover.VerifyWithJson(XunitLogger.Logs);
    }

    [Fact]
    public void Dispose_should_flush()
    {
        XunitLogger.Write("part1");
        XunitLogger.Write(" part2");
        XunitLogger.Flush();
        ObjectApprover.VerifyWithJson(XunitLogger.Logs);
    }

    [Fact]
    public void Write_after_Flush_should_throw()
    {
        XunitLogger.Flush();
        var exception = Assert.Throws<Exception>(() => XunitLogger.WriteLine());
        ObjectApprover.VerifyWithJson(exception);
    }

    public UsingStatic(ITestOutputHelper testOutput)
    {
    }
}