using Xunit;
using Xunit.Abstractions;

public class UniqueTestNameSample :
    XunitContextBase
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