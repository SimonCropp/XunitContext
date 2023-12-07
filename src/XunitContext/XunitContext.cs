namespace Xunit;

public static class XunitContext
{
    static AsyncLocal<Context?> local = new();
    internal static bool enableExceptionCapture;

    public static void EnableExceptionCapture()
    {
        if (enableExceptionCapture)
        {
            return;
        }

        enableExceptionCapture = true;
        AppDomain.CurrentDomain.FirstChanceException += (_, e) =>
        {
            if (local.Value == null)
            {
                return;
            }

            if (local.Value.flushed)
            {
                return;
            }

            local.Value.Exception = e.Exception;
        };
    }


    public static void Init()
    {
        var useGlobalLock = Trace.UseGlobalLock;
        Trace.UseGlobalLock = true;
        InnerInit();
        Trace.UseGlobalLock = useGlobalLock;
    }

    static void InnerInit()
    {
        #region writeRedirects

        Trace.Listeners.Clear();
        Trace.Listeners.Add(new TraceListener());
#if (NETFRAMEWORK)
        Debug.Listeners.Clear();
        Debug.Listeners.Add(new TraceListener());
#else
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
#endif
        TestWriter writer = new();
        Console.SetOut(writer);
        Console.SetError(writer);

        #endregion
    }

    public static void Write(string? value) =>
        Context.Write(value);

    public static void Write(object value) =>
        Context.Write(value);

    public static IReadOnlyList<string> Logs
    {
        get
        {
            var context = local.Value;
            if (context == null)
            {
                throw new("No current context.");
            }

            return context.LogMessages;
        }
    }

    public static void Write(char value) =>
        Context.Write(value);

    public static void WriteLine() =>
        Context.WriteLine();

    public static void WriteLine(string value) =>
        Context.WriteLine(value);

    public static void WriteLine(object value) =>
        Context.WriteLine(value);

    public static IReadOnlyList<string> Flush(bool clearAsyncLocal = true)
    {
        var context = local.Value;
        if (context == null)
        {
            throw new("No context to flush.");
        }

        context.Flush();
        var messages = context.LogMessages;
        if (clearAsyncLocal)
        {
            local.Value = null;
        }

        return messages;
    }

    public static Context Context
    {
        get
        {
            var context = local.Value;
            if (context != null)
            {
                return context;
            }

            context = new();
            local.Value = context;
            return context;
        }
    }

    public static Context Register(
        ITestOutputHelper output,
        [CallerFilePath] string sourceFile = "")
    {
        Guard.AgainstNull(output, nameof(output));
        Guard.AgainstNullOrEmpty(sourceFile, nameof(sourceFile));
        var existingContext = local.Value;

        if (existingContext == null)
        {
            Context context = new(output, sourceFile);
            local.Value = context;
            return context;
        }

        if (existingContext.TestOutput != null)
        {
            throw new($"A ITestOutputHelper has already been registered. Existing SourceFile: {existingContext.SourceFile}");
        }

        existingContext.TestOutput = output;
        existingContext.SourceFile = sourceFile;
        return existingContext;
    }
}