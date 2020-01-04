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

        /// <summary>
        /// Override the default parameter resolution.
        /// </summary>
        public void UseParameters(params object[] parameters)
        {
            Guard.AgainstNull(parameters, nameof(parameters));
            this.parameters = GetParameters(Test.TestCase,parameters);
        }

        static List<Parameter> empty = new List<Parameter>();

        #region Parameters
        static List<Parameter> GetParameters(ITestCase testCase)
        {
            return GetParameters(testCase, testCase.TestMethodArguments);
        }

        private static List<Parameter> GetParameters(ITestCase testCase, object[] arguments)
        {
            var method = testCase.TestMethod;
            var infos = method.Method.GetParameters().ToList();
            if (arguments == null || !arguments.Any())
            {
                if (infos.Count == 0)
                {
                    return empty;
                }

                throw NewNoArgumentsDetectedException();
            }

            var items = new List<Parameter>();

            for (var index = 0; index < infos.Count; index++)
            {
                items.Add(new Parameter(infos[index], arguments[index]));
            }

            return items;
        }
        #endregion

        static Exception NewNoArgumentsDetectedException()
        {
            return new Exception(@"No arguments detected for method with parameters.
This is most likely caused by using a parameter that Xunit cannot serialize.
Instead pass in a simple type as a parameter and construct the complex object inside the test.");
        }
    }
}