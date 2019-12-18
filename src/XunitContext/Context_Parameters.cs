using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Xunit
{
    public partial class Context
    {
        List<Parameter>? parameters;

        public IReadOnlyList<Parameter> Parameters
        {
            get => parameters ??= GetParameters(Test.TestCase);
        }

        static List<Parameter> empty = new List<Parameter>();

        #region Parameters
        static List<Parameter> GetParameters(ITestCase testCase)
        {
            var method = testCase.TestMethod;
            var infos = method.Method.GetParameters().ToList();
            var arguments = testCase.TestMethodArguments;
            if (arguments == null || !arguments.Any())
            {
                if (infos.Count != 0)
                {
                    throw new Exception("No arguments detected for method with parameters. This is most likely caused by using a parameter that Xunit cannot serialize. Instead pass in a simple type as a parameter and construct the complex object inside the test.");
                }
                return empty;
            }

            var items = new List<Parameter>();

            for (var index = 0; index < infos.Count; index++)
            {
                items.Add(new Parameter(infos[index], arguments[index]));
            }

            return items;
        }
        #endregion
    }
}