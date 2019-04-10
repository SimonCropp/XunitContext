using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

class LoggingContext
{
    public ITestOutputHelper TestOutput
    {
        get
        {
            ThrowIfFlushed();
            return testOutput;
        }
    }

    public StringBuilder Builder
    {
        get
        {
            ThrowIfFlushed();
            return builder;
        }
    }
    
    void ThrowIfFlushed()
    {
        if (Flushed)
        {
            throw new Exception("Logging context has been flushed.");
        }
    }

    public bool Flushed;
    public List<string> LogMessages = new List<string>();
    StringBuilder builder = new StringBuilder();
    ITestOutputHelper testOutput;

    public LoggingContext(ITestOutputHelper testOutput)
    {
        this.testOutput = testOutput;
    }
}