using Xunit;
using Xunit.Abstractions;

public class UniqueTestNameSample :
    XunitLoggingBase
{
    [Fact]
    public void Usage()
    {
        var currentGuid = Context.UniqueTestName;

        Context.WriteLine(currentGuid);
    }

    public UniqueTestNameSample(ITestOutputHelper output) :
        base(output)
    {
    }
}