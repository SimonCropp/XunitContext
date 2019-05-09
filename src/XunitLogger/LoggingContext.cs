using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using XunitLogger;

class LoggingContext
{
    public ITestOutputHelper TestOutput;
    GuidCounter GuidCounter = new GuidCounter();
    IntCounter IntCounter = new IntCounter();
    LongCounter LongCounter = new LongCounter();
    UIntCounter UIntCounter = new UIntCounter();
    ULongCounter ULongCounter = new ULongCounter();

    public uint NextUInt()
    {
        return UIntCounter.Next();
    }

    public int NextInt()
    {
        return IntCounter.Next();
    }

    public long NextLong()
    {
        return LongCounter.Next();
    }

    public ulong NextULong()
    {
        return ULongCounter.Next();
    }

    public Guid NextGuid()
    {
        return GuidCounter.Next();
    }

    public StringBuilder Builder;

    public void ThrowIfFlushed()
    {
        if (Flushed)
        {
            throw new Exception("Logging context has been flushed.");
        }
    }

    public bool Flushed;
    public List<string> LogMessages = new List<string>();

    public LoggingContext(ITestOutputHelper testOutput)
    {
        TestOutput = testOutput;
    }

    public void InitBuilder()
    {
        if (Builder == null)
        {
            Builder = new StringBuilder();
        }
    }
}