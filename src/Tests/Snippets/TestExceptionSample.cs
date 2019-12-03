using Xunit;
using Xunit.Abstractions;

namespace TestExceptionSample
{
    [Trait("Category", "Integration")]

    #region TestExceptionSample


    public class TestExceptionSample :
        XunitContextBase
    {
        [Fact]
        public void Usage()
        {
            //This tests will fail
            Assert.False(true);
        }

        public TestExceptionSample(ITestOutputHelper output) :
            base(output)
        {
        }

        public override void Dispose()
        {
            var theExceptionThrownByTest = Context.TestException;
            var testDisplayName = Context.Test.DisplayName;
            var testCase = Context.Test.TestCase;
            base.Dispose();
        }
    }

    [GlobalSetUp]
    public static class GlobalSetup
    {
        public static void Setup()
        {
            XunitContext.EnableExceptionCapture();
        }
    }

    #endregion
}