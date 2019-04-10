using System;
using System.Collections.Generic;
using Xunit.Abstractions;

public class XunitLoggingBase : IDisposable
{
    public XunitLoggingBase(ITestOutputHelper testOutput)
    {
        XunitLogger.Register(testOutput);
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