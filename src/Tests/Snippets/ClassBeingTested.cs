using System;
using System.Diagnostics;

static class ClassBeingTested
{
    public static void Method()
    {
        Trace.WriteLine("From Trace");
        Console.WriteLine("From Console");
        Debug.WriteLine("From Debug");
        Console.Error.WriteLine("From Console Error");
    }
}