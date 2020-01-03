using System;
using Xunit;
using Xunit.Abstractions;

public class StaticConstructor
{
    static StaticConstructor()
    {
        var type = typeof(XunitContext);
        Console.Write("a");
        Console.WriteLine("Foo");
    }

    [Fact]
    public void Verify()
    {
        Assert.EndsWith("StaticConstructor.cs", XunitContext.Context.SourceFile);
        var logs = XunitContext.Flush();

        Assert.Contains("aFoo" + Environment.NewLine, logs);
    }

    public StaticConstructor(ITestOutputHelper testOutput)
    {
        XunitContext.Register(testOutput);
    }
}