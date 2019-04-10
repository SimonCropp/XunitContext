using System;
using System.Collections.Generic;
using Xunit.Abstractions;

public class XunitLoggingBase : IDisposable
{
    public XunitLoggingBase(ITestOutputHelper output)
    {
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

    public static IReadOnlyCollection<string> Logs => XunitLogger.Logs;

    public virtual void Dispose()
    {
        XunitLogger.Flush();
    }
}