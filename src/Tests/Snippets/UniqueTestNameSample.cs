using Xunit;
using Xunit.Abstractions;

public class UniqueTestNameSample :
    XunitContextBase
{
    [Fact]
    public void Usage()
    {
        var testName = Context.UniqueTestName;

        Context.WriteLine(testName);
    }

    public UniqueTestNameSample(ITestOutputHelper output) :
        base(output)
    {
    }
}