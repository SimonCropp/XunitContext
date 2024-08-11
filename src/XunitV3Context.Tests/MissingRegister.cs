public class MissingRegister
{
    [Fact]
    public void CurrentTest()
    {
        var exception = Assert.Throws<Exception>(() => XunitContext.Context.Test.TestDisplayName);
        Assert.Equal(Context.MissingTestOutput, exception.Message);
    }
}