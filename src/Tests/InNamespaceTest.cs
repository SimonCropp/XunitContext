using Xunit;
using Xunit.Abstractions;

namespace MyNamespace.Bar
{
    public class InNamespaceTest :
        XunitContextBase
    {
        [Fact]
        public void Usage()
        {
            Assert.Equal("InNamespaceTest.Usage", Context.UniqueTestName);
        }

        public InNamespaceTest(ITestOutputHelper output) :
            base(output)
        {
        }
    }
}