using System;

static class Extensions
{
    public static string TrimTrailingNewline(this string value)
    {
        return value.Substring(0, value.Length - Environment.NewLine.Length);
    }
}