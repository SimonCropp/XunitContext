using Xunit.Sdk;
using Xunit.v3;

namespace Xunit;

public partial class Context
{
    ITest? test;


    public ITest Test
    {
        get
        {
            InitTest();

            return test!;
        }
    }

    MethodInfo? methodInfo;

    public MethodInfo MethodInfo
    {
        get
        {
            InitTest();
            return methodInfo!;
        }
    }

    Type? testType;

    public Type TestType
    {
        get
        {
            InitTest();
            return testType!;
        }
    }

    void InitTest()
    {
        if (test != null)
        {
            return;
        }

        if (TestOutput == null)
        {
            throw new(MissingTestOutput);
        }

        test = TestContext.Current.Test!;
        methodInfo = ((IXunitTestMethod)test.TestCase.TestMethod!).Method;
        testType = ((IXunitTestClass)test.TestCase.TestClass!).Class;
    }

    public const string MissingTestOutput = "ITestOutputHelper has not been set. It is possible that the call to `XunitContext.Register()` is missing, or the current test does not inherit from `XunitContextBase`.";
}