using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

static class GlobalSetupRunner
{
    static GlobalSetupRunner()
    {
        var strongNamesToIgnore =
            new List<string>
            {
                // mscorlib
                "b77a5c561934e089",
                // ReSharper
                "1010a0d8d6380325",
                // System
                "7cec85d7bea7798e",
                "b03f5f7f11d50a3a",
                "cc7b13ffcd2ddd51",
                //newtonsoft
                "30ad4fe6b2a6aeed",
                //xunit
                "8d05b1bb7a6fdb6c",
                //nuget
                "31bf3856ad364e35",
                //aspnet + ef
                "adb9793829ddae60"
            };

        var startNew = Stopwatch.StartNew();
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var fullName = assembly.FullName;
            if (fullName.StartsWith("XunitContext,"))
            {
                continue;
            }

            if (assembly.IsDynamic)
            {
                continue;
            }

            if (assembly.GlobalAssemblyCache)
            {
                continue;
            }

            if (strongNamesToIgnore.Any(x => fullName.Contains(x)))
            {
                continue;
            }

            if (assembly.GetReferencedAssemblies().All(x => x.Name != "XunitContext"))
            {
                continue;
            }

            foreach (var globalSetup in assembly.GetTypes().Where(x => x.Name == "XunitGlobalSetup"))
            {
                Invoke(globalSetup);
            }
        }

        startNew.Stop();
    }

    public static void Run()
    {
    }

    static void Invoke(Type globalSetupType)
    {
        var method = globalSetupType.GetMethod(
            name: "Setup",
            bindingAttr: BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            types: Array.Empty<Type>(),
            modifiers: null);
        if (method == null)
        {
            throw new Exception("Expected `XunitGlobalSetup` to contain a method named `Setup`");
        }

        if (method.ReturnType != typeof(void))
        {
            throw new Exception("Expected `XunitGlobalSetup.Setup` to have no return value.");
        }

        if (!method.IsStatic)
        {
            throw new Exception("Expected `XunitGlobalSetup.Setup` to be static.");
        }

        if (!method.IsPublic)
        {
            throw new Exception("Expected `XunitGlobalSetup.Setup` to be public.");
        }

        method.Invoke(null, null);
    }
}