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

#if NET8_0_OR_GREATER
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "test")]
        static extern ref ITest GetTest(TestOutputHelper? c);
        test = GetTest((TestOutputHelper) TestOutput);
#else
        test = (ITest) GetTestMethod(TestOutput)
            .GetValue(TestOutput)!;
#endif
        var method = (ReflectionMethodInfo) test.TestCase.TestMethod.Method;
        var type = (ReflectionTypeInfo) test.TestCase.TestMethod.TestClass.Class;
        methodInfo = method.MethodInfo;
        testType = type.Type;
    }

    public const string MissingTestOutput = "ITestOutputHelper has not been set. It is possible that the call to `XunitContext.Register()` is missing, or the current test does not inherit from `XunitContextBase`.";

#if !NET8_0_OR_GREATER
    static FieldInfo? cachedTestMember;

    static FieldInfo GetTestMethod(ITestOutputHelper testOutput)
    {
        if (cachedTestMember != null)
        {
            return cachedTestMember;
        }

        var testOutputType = testOutput.GetType();
        cachedTestMember = testOutputType.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
        if (cachedTestMember == null)
        {
            throw new($"Unable to find 'test' field on {testOutputType.FullName}");
        }

        return cachedTestMember;
    }
#endif
}