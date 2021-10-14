using System;
using Xunit;

public class MissingRegister
{
    [Fact]
    public void CurrentTest()
    {
        var exception = Assert.Throws<Exception>(() => XunitContext.Context.Test.DisplayName);
        Assert.Equal(Context.MissingTestOutput, exception.Message);
    }
}