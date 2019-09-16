using System;
using Xunit;
using XunitLogger;

public class MissingRegister
{
    [Fact]
    public void CurrentTest()
    {
        var exception = Assert.Throws<Exception>(() => XunitLogging.Context.Test.DisplayName);
        Assert.Equal(Context.MissingTestOutput, exception.Message);
    }
}