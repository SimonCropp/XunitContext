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
                    if (testMember == null)
                    {
                        var testOutputType = TestOutput.GetType();
                        testMember = testOutputType.GetField("test", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (testMember == null)
                        {
                            throw new Exception($"Unable to find 'test' field on {testOutputType.FullName}");
                        }
                    }

                    test = (ITest) testMember.GetValue(TestOutput);
                }

                return test;
            }
        }
    }
}