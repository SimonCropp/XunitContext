using System;
using Xunit;
using Xunit.Abstractions;

public class StaticConstructor
{
    static StaticConstructor()
    {
        var type = typeof(XunitLogging);
        Console.Write("a");
        Console.WriteLine("Foo");
    }

    [Fact]
    public void Verify()
    {
        var logs = XunitLogging.Flush();
        ObjectApprover.Verify(logs);
    }

    public StaticConstructor(ITestOutputHelper testOutput)
    {
        XunitLogging.Register(testOutput);
    }
}