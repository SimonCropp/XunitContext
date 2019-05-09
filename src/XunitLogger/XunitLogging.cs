using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Xunit.Abstractions;
using XunitLogger;

public static class XunitLogging
{
    static AsyncLocal<Context> loggingContext = new AsyncLocal<Context>();

    #region writeRedirects

    static XunitLogging()
    {
        Trace.Listeners.Clear();
        Trace.Listeners.Add(new TraceListener());
#if (NETSTANDARD)
        DebugPoker.Overwrite(
            text =>
            {
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                if (text.EndsWith(Environment.NewLine))
                {
                    WriteLine(text.TrimTrailingNewline());
                    return;
                }

                Write(text);
            });
#else
        Debug.Listeners.Clear();
        Debug.Listeners.Add(new TraceListener());
#endif
        var writer = new TestWriter();
        Console.SetOut(writer);
        Console.SetError(writer);
    }

    #endregion

    public static void Write(string value)
    {
        Guard.AgainstNull(value, nameof(value));
        Context.Write(value);
    }

    public static IReadOnlyList<string> Logs => Context.LogMessages;

    public static void Write(char value)
    {
        Context.Write(value);
    }

    public static void WriteLine()
    {
        Context.WriteLine();
    }

    public static void WriteLine(string value)
    {
        Context.WriteLine(value);
    }

    public static void Flush()
    {
        Context.Flush();
    }

   public static Context Context
    {
        get
        {
            var context = loggingContext.Value;
            if (context != null)
            {
                return context;
            }

            throw new Exception("An attempt was made to write to Trace or Console, however no logging context was found. Either XunitLogger.Register(ITestOutputHelper) needs to be called at test startup, or have the test inherit from XunitLoggingBase.");
        }
    }

    public static Context Register(ITestOutputHelper output)
    {
        Guard.AgainstNull(output, nameof(output));
        var context = new Context(output);
        loggingContext.Value = context;
        return context;
    }
}