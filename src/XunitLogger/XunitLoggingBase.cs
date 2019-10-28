using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit.Abstractions;
using XunitLogger;

public abstract class XunitLoggingBase :
    IDisposable
{
    static ConcurrentDictionary<Type, string> filePathCacheDictionary = new ConcurrentDictionary<Type, string>();

    public ITestOutputHelper Output { get; }
    public Context Context { get; }

    protected XunitLoggingBase(ITestOutputHelper output)
    {
        Guard.AgainstNull(output, nameof(output));
        Output = output;
        var sourceFile = filePathCacheDictionary.GetOrAdd(
            GetType(),
            type =>
            {
                var trace = new StackTrace(3, true);
                return TestFileFinder.WalkStackTraceForFileWithConstructor(trace);
            });
        Context = XunitLogging.Register(output, sourceFile);
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