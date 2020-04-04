using System;
using Xunit;
using Xunit.Abstractions;

[Trait("Category", "Integration")]
public class TestException_Sync :
    XunitContextBase
{
    [Fact]
    public void Root()
    {
        throw new Exception("root");
    }

    [Fact]
    public void AssertThrows()
    {
        Assert.Throws<Exception>(MethodThatThrows);
    }

    [Fact]
    public void Caught()
    {
        try
        {
            MethodThatThrows();
        }
        catch
        {
        }
    }

    [Fact]
    public void FailedAssert()
    {
        Assert.True(false);
    }

    [Fact]
    public void Nested()
    {
        MethodThatThrows();
    }

    static void MethodThatThrows()
    {
        throw new Exception("nested");
    }

    public TestException_Sync(ITestOutputHelper output) :
        base(output)
    {
    }

    public override void Dispose()
    {
        var theExceptionThrownByTest = Context.TestException;
        base.Dispose();
    }
}