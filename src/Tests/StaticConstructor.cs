using System;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

[UsesVerify]
public class StaticConstructor
{
    static StaticConstructor()
    {
        var type = typeof(XunitContext);
        Console.Write("a");
        Console.WriteLine("Foo");
    }

    [Fact]
    public Task Verify()
    {
        Assert.EndsWith("StaticConstructor.cs", XunitContext.Context.SourceFile);
        var logs = XunitContext.Flush(false);
        return Verifier.Verify(logs);
    }

    public StaticConstructor(ITestOutputHelper testOutput)
    {
        XunitContext.Register(testOutput);
    }
}