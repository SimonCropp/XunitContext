using Xunit;
using Xunit.Abstractions;

namespace TheNamespace
{
    public class TestContextWithNamespace :
        XunitContextBase
    {
        [Fact]
        public void CurrentTest()
        {
            Assert.Equal("TestContextWithNamespace", Context.ClassName);
            Assert.Equal("CurrentTest", Context.MethodName);
            Assert.EndsWith("TestContextWithNamespace.cs", Context.SourceFile);
            Assert.EndsWith("TestContextWithNamespace.CurrentTest", Context.UniqueTestName);
        }

        public TestContextWithNamespace(ITestOutputHelper testOutput) :
            base(testOutput)
        {
        }
    }
}