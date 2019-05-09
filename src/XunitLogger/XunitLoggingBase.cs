using System;
using System.Collections.Generic;
using Xunit.Abstractions;

public abstract class XunitLoggingBase :
    IDisposable
{
    public ITestOutputHelper Output { get; }

    public XunitLoggingBase(ITestOutputHelper output)
    {
        Guard.AgainstNull(output, nameof(output));
        Output = output;
        XunitLogging.Register(output);
    }

    public void WriteLine(string value)
    {
        XunitLogging.WriteLine(value);
    }
    public void WriteLine()
    {
        XunitLogging.WriteLine();
    }

    public void Write(string value)
    {
        XunitLogging.Write(value);
    }

    public uint NextUInt()
    {
        return XunitLogging.NextUInt();
    }

    public int NextInt()
    {
        return XunitLogging.NextInt();
    }

    public long NextLong()
    {
        return XunitLogging.NextLong();
    }

    public ulong NextULong()
    {
        return XunitLogging.NextULong();
    }

    public Guid NextGuid()
    {
        return XunitLogging.NextGuid();
    }


    public static IReadOnlyList<string> Logs => XunitLogging.Logs;

    public virtual void Dispose()
    {
        XunitLogging.Flush();
    }
}