﻿using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace Xunit
{
    public partial class Context
    {
        string? uniqueTestName;

        public string ClassName
        {
            get => Test.TestCase.TestMethod.TestClass.Class.ClassName();
        }

        public string MethodName
        {
            get => Test.TestCase.TestMethod.Method.Name;
        }

        public string UniqueTestName
        {
            get => uniqueTestName ??= GetUniqueTestName(Test.TestCase);
        }

        #region UniqueTestName
        string GetUniqueTestName(ITestCase testCase)
        {
            var method = testCase.TestMethod;
            var name = $"{method.TestClass.Class.ClassName()}.{method.Method.Name}";
            if (!Parameters.Any())
            {
                return name;
            }

            var builder = new StringBuilder();
            foreach (var parameter in Parameters)
            {
                builder.Append($"{parameter.Info.Name}=");
                if (parameter.Value == null)
                {
                    builder.Append("null_");
                    continue;
                }

                builder.Append($"{parameter.Value}_");
            }

            builder.Length -= 1;

            return $"{name}_{builder}";
        }
        #endregion
    }
}