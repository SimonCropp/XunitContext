using System;
using System.Reflection;
using Xunit.Abstractions;

namespace XunitLogger
{
    public partial class Context
    {
        ITest test;

        static FieldInfo testMember;
        public ITest Test
        {
            get
            {
                if (test == null)
                {
                    InitTestMethod();

                    test = (ITest) testMember.GetValue(TestOutput);
                }

                return test;
            }
        }
        
        public static string MissingTestOutput = "ITestOutputHelper has not been set. It is possible that the call to `XunitLogging.Register()` is missing, or the current test does not inherit from `XunitLoggingBase`.";
        void InitTestMethod()
        {
            if (TestOutput == null)
            {
                throw new Exception(MissingTestOutput);
            }
            if (testMember != null)
            {
                return;
            }
            var testOutputType = TestOutput.GetType();
            testMember = testOutputType.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
            if (testMember == null)
            {
                throw new Exception($"Unable to find 'test' field on {testOutputType.FullName}");
            }
        }
    }
}