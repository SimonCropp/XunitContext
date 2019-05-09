using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Xunit.Abstractions;

public static class XunitLogging
{
    static AsyncLocal<LoggingContext> loggingContext = new AsyncLocal<LoggingContext>();

    public static ConcurrentBag<Func<string, bool>> Filters = new ConcurrentBag<Func<string, bool>>();

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
        var context = GetContext();
        lock (context)
        {
            context.ThrowIfFlushed();
            context.InitBuilder();
            context.Builder.Append(value);
        }
    }

    public static uint NextUInt()
    {
        return GetContext().UIntCounter.Next();
    }

    public static int NextInt()
    {
        return GetContext().IntCounter.Next();
    }

    public static long NextLong()
    {
        return GetContext().LongCounter.Next();
    }

    public static ulong NextULong()
    {
        return GetContext().ULongCounter.Next();
    }

    public static Guid NextGuid()
    {
        return GetContext().GuidCounter.Next();
    }

    public static IReadOnlyList<string> Logs => GetContext().LogMessages;

    public static void Write(char value)
    {
        var context = GetContext();
        lock (context)
        {
            context.ThrowIfFlushed();
            context.InitBuilder();
            context.Builder.Append(value);
        }
    }

    public static void WriteLine()
    {
        var context = GetContext();

        lock (context)
        {
            context.ThrowIfFlushed();
            var builder = context.Builder;
            if (context.Builder == null)
            {
                context.LogMessages.Add("");
                context.TestOutput.WriteLine("");
                return;
            }

            var message = builder.ToString();
            context.Builder = null;
            if (ShouldFilterOut(message))
            {
                return;
            }

            context.LogMessages.Add(message);
            context.TestOutput.WriteLine(message);
        }
    }

    public static void WriteLine(string value)
    {
        Guard.AgainstNull(value, nameof(value));
        var context = GetContext();

        lock (context)
        {
            context.ThrowIfFlushed();
            var builder = context.Builder;
            if (context.Builder == null)
            {
                if (ShouldFilterOut(value))
                {
                    return;
                }

                context.LogMessages.Add(value);
                context.TestOutput.WriteLine(value);
                return;
            }

            builder.Append(value);
            var message = builder.ToString();
            context.Builder = null;
            if (ShouldFilterOut(message))
            {
                return;
            }

            context.LogMessages.Add(message);
            context.TestOutput.WriteLine(message);
        }
    }

    static bool ShouldFilterOut(string message)
    {
        foreach (var filter in Filters)
        {
            if (!filter(message))
            {
                return true;
            }
        }

        return false;
    }

    public static void Flush()
    {
        var context = GetContext();
        lock (context)
        {
            if (context.Flushed)
            {
                return;
            }

            context.Flushed = true;
            var builder = context.Builder;
            if (builder == null)
            {
                return;
            }

            var message = builder.ToString();
            context.Builder = null;
            if (ShouldFilterOut(message))
            {
                return;
            }

            context.LogMessages.Add(message);
            context.TestOutput.WriteLine(message);
        }
    }

    static LoggingContext GetContext()
    {
        var context = loggingContext.Value;
        if (context != null)
        {
            return context;
        }

        throw new Exception("An attempt was made to write to Trace or Console, however no logging context was found. Either XunitLogger.Register(ITestOutputHelper) needs to be called at test startup, or have the test inherit from XunitLoggingBase.");
    }

    public static void Register(ITestOutputHelper output)
    {
        Guard.AgainstNull(output, nameof(output));
        loggingContext.Value = new LoggingContext(output);
    }
}