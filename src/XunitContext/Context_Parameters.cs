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
            var arguments = testCase.TestMethodArguments;
            if (arguments == null || !arguments.Any())
            {
                return empty;
            }

            var items = new List<Parameter>();

            var method = testCase.TestMethod;
            var infos = method.Method.GetParameters().ToList();
            for (var index = 0; index < infos.Count; index++)
            {
                items.Add(new Parameter(infos[index], arguments[index]));
            }

            return items;
        }
        #endregion
    }
}