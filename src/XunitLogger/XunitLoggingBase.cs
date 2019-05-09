using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using XunitLogger;

public abstract class XunitLoggingBase :
    IDisposable
{
    public ITestOutputHelper Output { get; }
    public Context Context { get; }

    public XunitLoggingBase(ITestOutputHelper output)
    {
        Guard.AgainstNull(output, nameof(output));
        Output = output;
        Context = XunitLogging.Register(output);
    }

    public void WriteLine(string value)
    {
        Context.WriteLine(value);
    }

    public void WriteLine()
    {
        Context.WriteLine();
    }

    public void Write(string value)
    {
        Context.Write(value);
    }

    public IReadOnlyList<string> Logs => Context.LogMessages;

    public virtual void Dispose()
    {
        Context.Flush();
    }
}