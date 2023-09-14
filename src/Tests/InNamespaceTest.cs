
namespace MyNamespace.Bar;

public class InNamespaceTest(ITestOutputHelper output) :
    XunitContextBase(output)
{
    [Fact]
    public void Usage() =>
        Assert.Equal("InNamespaceTest.Usage", Context.UniqueTestName);
}