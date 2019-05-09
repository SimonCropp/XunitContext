using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;
using XunitLogger;

class LoggingContext
{
    public readonly ITestOutputHelper TestOutput;
    public readonly GuidCounter GuidCounter = new GuidCounter();
    public readonly IntCounter IntCounter = new IntCounter();
    public readonly   LongCounter LongCounter = new LongCounter();
    public readonly   UIntCounter UIntCounter = new UIntCounter();
    public readonly  ULongCounter ULongCounter = new ULongCounter();

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