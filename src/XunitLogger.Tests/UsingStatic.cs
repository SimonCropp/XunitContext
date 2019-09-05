using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class UsingStatic
{
    [Fact]
    public void Counters()
    {
        var context = XunitLogging.Context;
        var foo = new
        {
            int1 = context.NextInt(),
            int2 = context.NextInt(),
            uint1 = context.NextUInt(),
            uint2 = context.NextUInt(),
            long1 = context.NextLong(),
            long2 = context.NextLong(),
            ulong1 = context.NextULong(),
            ulong2 = context.NextULong(),
            guid1 = context.NextGuid(),
            guid2 = context.NextGuid(),
        };
        ObjectApprover.Verify(foo);
    }

    [Fact]
    public void Overwrites()
    {
        Console.WriteLine("from Console");
        Debug.WriteLine("from Debug");
        Trace.WriteLine("from Trace");
        var logs = XunitLogging.Flush();
        ObjectApprover.Verify(logs);
    }

    [Fact]
    public void CurrentTest()
    {
        Assert.Equal("UsingStatic.CurrentTest", XunitLogging.Context.Test.DisplayName);
    }

    [Fact]
    public void Null()
    {
        XunitLogging.WriteLine("XunitLogger.WriteLine");
        XunitLogging.WriteLine();
        XunitLogging.WriteLine("Console.WriteLine(null)");
        Console.WriteLine((string) null);
        XunitLogging.WriteLine("Debug.WriteLine(null)");
        Debug.WriteLine(null);
        XunitLogging.WriteLine("Debug.Write(null)");
        Debug.Write(null);
        XunitLogging.WriteLine("Trace.WriteLine(null)");
        Trace.WriteLine(null);
        XunitLogging.WriteLine("Trace.Write(null)");
        Trace.Write(null);
        var logs = XunitLogging.Flush();
        ObjectApprover.Verify(logs);
    }

    [Fact]
    public void Write_lines()
    {
        XunitLogging.Write("part1");
        XunitLogging.Write(" part2");
        XunitLogging.WriteLine();
        XunitLogging.WriteLine("part3");
        var logs = XunitLogging.Flush();
        ObjectApprover.Verify(logs);
    }

    [Fact]
    public async Task Async()
    {
        await Task.Delay(1);
        XunitLogging.WriteLine("part1");
        await Task.Delay(1).ConfigureAwait(false);
        XunitLogging.WriteLine("part2");
        await Task.Delay(1).ConfigureAwait(false);
        var logs = XunitLogging.Flush();
        ObjectApprover.Verify(logs);
    }

    public UsingStatic(ITestOutputHelper testOutput)
    {
        XunitLogging.Register(testOutput);
    }
}