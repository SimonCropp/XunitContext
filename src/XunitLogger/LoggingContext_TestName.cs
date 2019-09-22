using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace XunitLogger
{
    public partial class Context
    {
        string? uniqueTestName;

        public string UniqueTestName
        {
            get
            {
                if (uniqueTestName == null)
                {
                    uniqueTestName = GetUniqueTestName(Test.TestCase);
                }

                return uniqueTestName;
            }
        }
        
        static string GetUniqueTestName(ITestCase testCase)
        {
            var arguments = testCase.TestMethodArguments;
            var method = testCase.TestMethod;
            var name = $"{method.TestClass.Class.Name}.{method.Method.Name}";
            if (arguments == null || !arguments.Any())
            {
                return name;
            }

            var builder = new StringBuilder();
            var parameterInfos = method.Method.GetParameters().ToList();
            for (var index = 0; index < parameterInfos.Count; index++)
            {
                var parameterInfo = parameterInfos[index];
                var argument = arguments[index];
                if (argument == null)
                {
                    builder.Append($"{parameterInfo.Name}=null_");
                    continue;
                }

                builder.Append($"{parameterInfo.Name}={argument}_");
            }

            builder.Length -= 1;

            return $"{name}_{builder}";
        }
    }
}