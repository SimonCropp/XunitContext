using System;
using System.Collections.Generic;
using Xunit.Abstractions;

public abstract class XunitLoggingBase : IDisposable
{
    public ITestOutputHelper Output { get; }

    public XunitLoggingBase(ITestOutputHelper output)
    {
        Guard.AgainstNull(output, nameof(output));
        Output = output;
        XunitLogger.Register(output);
    }

    public void WriteLine(string value = null)
    {
        XunitLogger.WriteLine(value);
    }

    public void Write(string value)
    {
        XunitLogger.Write(value);
    }

    public static IReadOnlyList<string> Logs => XunitLogger.Logs;

    public virtual void Dispose()
    {
        XunitLogger.Flush();
    }
}