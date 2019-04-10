using Xunit;
using Xunit.Abstractions;

public class TestBaseSample :
    XunitLoggingBase
{
    [Fact]
    public void Write_lines()
    {
        WriteLine("The log message");
    }

    public TestBaseSample(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}