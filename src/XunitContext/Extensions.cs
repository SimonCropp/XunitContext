using System;
using Xunit.Abstractions;

static class Extensions
{
    public static string TrimTrailingNewline(this string value)
    {
        return value.Substring(0, value.Length - Environment.NewLine.Length);
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