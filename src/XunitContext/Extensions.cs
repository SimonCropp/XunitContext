using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

static class Extensions
{
    public static string TrimTrailingNewline(this string value)
    {
        return value.Substring(0, value.Length - Environment.NewLine.Length);
    }

    public static MethodBase GetRealMethod(this MethodBase method)
    {
        var declaringType = method.DeclaringType;
        if (!typeof(IAsyncStateMachine).IsAssignableFrom(declaringType))
        {
            return method;
        }
        var realType = declaringType.DeclaringType;
        foreach (var methodInfo in realType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var stateMachineAttribute = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>();
            if (stateMachineAttribute == null)
            {
                continue;
            }
            if (stateMachineAttribute.StateMachineType == declaringType)
            {
                return methodInfo;
            }
        }

        return method;
    }
    public static string ClassName(this ITypeInfo value)
    {
        var name = value.Name;
        var indexOf = name.LastIndexOf('.');
        if (indexOf == -1)
        {
            return name;
        }

        return name.Substring(indexOf + 1, name.Length - indexOf - 1);
    }
}