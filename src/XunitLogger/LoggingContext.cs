using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

class LoggingContext
{
    public readonly ITestOutputHelper TestOutput;

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