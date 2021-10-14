using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

[UsesVerify]
public class UsingStatic
{
    [Fact]
    public Task Overwrites()
    {
        Console.WriteLine("from Console");
        Debug.WriteLine("from Debug");
        Trace.WriteLine("from Trace");
        var logs = XunitContext.Flush(false);
        return Verifier.Verify(logs);
    }

    //[Fact]
    //public void CurrentTest()
    //{
    //    Assert.Equal("CurrentTest", XunitContext.Context.Test.DisplayName);
    //}

    [Fact]
    public Task Null()
    {
        XunitContext.WriteLine("XunitLogger.WriteLine");
        XunitContext.WriteLine();
        XunitContext.WriteLine("Console.WriteLine(null)");
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        Console.WriteLine((string) null);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        XunitContext.WriteLine("Debug.WriteLine(null)");
        Debug.WriteLine(null);
        XunitContext.WriteLine("Debug.Write(null)");
        Debug.Write(null);
        XunitContext.WriteLine("Trace.WriteLine(null)");
        Trace.WriteLine(null);
        XunitContext.WriteLine("Trace.Write(null)");
        Trace.Write(null);
        var logs = XunitContext.Flush(false);
        return Verifier.Verify(logs);
    }

    [Fact]
    public Task Write_lines()
    {
        XunitContext.Write("part1");
        XunitContext.Write(" part2");
        XunitContext.WriteLine();
        XunitContext.WriteLine("part3");
        var logs = XunitContext.Flush(false);
        return Verifier.Verify(logs);
    }

    [Fact]
    public async Task Async()
    {
        await Task.Delay(1);
        XunitContext.WriteLine("part1");
        await Task.Delay(1);
        XunitContext.WriteLine("part2");
        await Task.Delay(1);
        var logs = XunitContext.Flush(false);
        await Verifier.Verify(logs);
    }

    [Fact]
    public Task Split_Lines()
    {
        XunitContext.Write("A\nB");
        XunitContext.Write("C\r\nD");
        XunitContext.Write("E\rF\r");
        XunitContext.Write("\n");
        XunitContext.Write('\n');
        XunitContext.Write('\n');
        XunitContext.Write('\r');
        XunitContext.Write('\n');
        XunitContext.Write("\r\n");
        XunitContext.Write("G\r\nH\nI");
        XunitContext.Write("\n\r\n");
        XunitContext.Write('J');
        var logs = XunitContext.Flush(false);
        // Verifier normalises '\r' characters, so we need to manually detect embedded '\r' from above
        Assert.Equal("DE\rF", logs[2]);
        return Verifier.Verify(logs);
    }

    public UsingStatic(ITestOutputHelper testOutput)
    {
        XunitContext.Register(testOutput);
    }
}