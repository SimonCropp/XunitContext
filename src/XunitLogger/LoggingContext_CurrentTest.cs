using System;
using System.Reflection;
using Xunit.Abstractions;

namespace XunitLogger
{
    public partial class Context
    {
        ITest? test;

        static FieldInfo? cachedTestMember;

        public ITest Test
        {
            get
            {
                if (test == null)
                {
                    var testMember = GetTestMethod();

                    test = (ITest) testMember.GetValue(TestOutput);
                }

                return test;
            }
        }

        public static string MissingTestOutput = "ITestOutputHelper has not been set. It is possible that the call to `XunitLogging.Register()` is missing, or the current test does not inherit from `XunitLoggingBase`.";
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