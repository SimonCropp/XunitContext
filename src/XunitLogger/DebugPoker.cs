using System;
using System.Diagnostics;
using System.Reflection;

static class DebugPoker
{
    public static void Overwrite(Action<string> action)
    {
        var flags = BindingFlags.Static | BindingFlags.NonPublic;
        var writeCoreHook = typeof(Debug).GetField("s_WriteCore", flags);
        if (writeCoreHook != null)
        {
            writeCoreHook.SetValue(null, action);
            return;
        }

        var debugProviderType = Type.GetType("System.Diagnostics.DebugProvider");
        if (debugProviderType != null)
        {
            writeCoreHook = debugProviderType.GetField("s_WriteCore", flags);
            if (writeCoreHook != null)
            {
                writeCoreHook.SetValue(null, action);
                return;
            }
        }

        throw new Exception("Unable to find s_WriteCore field in either Debug nor DebugProvider. It is possible the current runtime is not supported.");
    }
}