using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Xunit
{
    public partial class Context
    {
        ITest? test;

        static FieldInfo? cachedTestMember;

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
            test = (ITest) GetTestMethod().GetValue(TestOutput);
            var method = (ReflectionMethodInfo) test.TestCase.TestMethod.Method;
            var type = (ReflectionTypeInfo) test.TestCase.TestMethod.TestClass.Class;
            methodInfo = method.MethodInfo;
            testType = type.Type;
        }

        public static string MissingTestOutput = "ITestOutputHelper has not been set. It is possible that the call to `XunitContext.Register()` is missing, or the current test does not inherit from `XunitContextBase`.";

        FieldInfo GetTestMethod()
        {
            if (TestOutput == null)
            {
                throw new Exception(MissingTestOutput);
            }

            if (cachedTestMember != null)
            {
                return cachedTestMember;
            }

            var testOutputType = TestOutput.GetType();
            cachedTestMember = testOutputType.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            if (cachedTestMember == null)
            {
                throw new Exception($"Unable to find 'test' field on {testOutputType.FullName}");
            }

            return cachedTestMember;
        }
    }
}