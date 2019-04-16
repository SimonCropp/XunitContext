using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ObjectApproval;
using Xunit;
using Xunit.Abstractions;

public class UsingStatic
{
    [Fact]
    public void Overwrites()
    {
        Console.WriteLine("from Console");
        Debug.WriteLine("from Debug");
        Trace.WriteLine("from Trace");
        XunitLogger.Flush();
        ObjectApprover.VerifyWithJson(XunitLogger.Logs);
    }
    [Fact]
    public void Null()
    {
        XunitLogger.WriteLine("XunitLogger.WriteLine");
        XunitLogger.WriteLine();
        XunitLogger.WriteLine("Console.WriteLine(null)");
        Console.WriteLine((string)null);
        XunitLogger.WriteLine("Debug.WriteLine(null)");
        Debug.WriteLine(null);
        XunitLogger.WriteLine("Debug.Write(null)");
        Debug.Write(null);
        XunitLogger.WriteLine("Trace.WriteLine(null)");
        Trace.WriteLine(null);
        XunitLogger.WriteLine("Trace.Write(null)");
        Trace.Write(null);
        XunitLogger.Flush();
        ObjectApprover.VerifyWithJson(XunitLogger.Logs);
    }

    [Fact]
    public void Write_lines()
    {
        XunitLogger.Write("part1");
        XunitLogger.Write(" part2");
        XunitLogger.WriteLine();
        XunitLogger.WriteLine("part3");
        XunitLogger.Flush();
        ObjectApprover.VerifyWithJson(XunitLogger.Logs);
    }

    [Fact]
    public async Task Async()
    {
        await Task.Delay(1);
        XunitLogger.WriteLine("part1");
        await Task.Delay(1).ConfigureAwait(false);
        XunitLogger.WriteLine("part2");
        await Task.Delay(1).ConfigureAwait(false);
        XunitLogger.Flush();
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
        XunitLogger.Register(testOutput);
    }
}