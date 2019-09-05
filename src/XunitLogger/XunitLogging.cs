using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Threading;
using Xunit.Abstractions;
using XunitLogger;

public static class XunitLogging
{
    static AsyncLocal<Context> loggingContext = new AsyncLocal<Context>();
    static bool enableExceptionCapture;

    public static void EnableExceptionCapture()
    {
        if (enableExceptionCapture)
        {
            return;
        }

        enableExceptionCapture = true;
        AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
        {
            if (loggingContext.Value == null)
            {
                return;
            }

            if (loggingContext.Value.flushed)
            {
                return;
            }
            loggingContext.Value.Exception = e.Exception;
        };
    }


    public static void Init()
    {
        var useGlobalLock = Trace.UseGlobalLock;
        Trace.UseGlobalLock = true;
        InnerInit();
        Trace.UseGlobalLock = useGlobalLock;
    }

    private static void InnerInit()
    {
        #region writeRedirects

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

        #endregion
    }

    public static void Write(string value)
    {
        Guard.AgainstNull(value, nameof(value));
        Context.Write(value);
    }

    public static IReadOnlyList<string> Logs
    {
        get
        {
            var context = loggingContext.Value;
            if (context == null)
            {
                throw new Exception("No current context.");
            }

            return context.LogMessages;
        }
    }

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

    public static IReadOnlyList<string> Flush()
    {
        var context = loggingContext.Value;
        if (context == null)
        {
            throw new Exception("No context to flush.");
        }

        context.Flush();
        var messages = context.LogMessages;
        loggingContext.Value = null;
        return messages;
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

            context = new Context();
            loggingContext.Value = context;
            return context;
        }
    }

    public static Context Register(ITestOutputHelper output,
        [CallerFilePath] string sourceFilePath = "")
    {
        Guard.AgainstNull(output, nameof(output));
        Guard.AgainstNullOrEmpty(sourceFilePath, nameof(sourceFilePath));
        var existingContext = loggingContext.Value;

        if (existingContext == null)
        {
            var context = new Context(output, sourceFilePath);
            loggingContext.Value = context;
            return context;
        }

        if (existingContext.TestOutput != null)
        {
            throw new Exception("A ITestOutputHelper has already been registered.");
        }

        existingContext.TestOutput = output;
        return existingContext;
    }
}