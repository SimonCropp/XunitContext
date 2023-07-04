namespace Xunit;

public partial class Context
{
    string? uniqueTestName;

    public string ClassName => Test.TestCase.TestMethod.TestClass.Class.ClassName();

    public string MethodName => Test.TestCase.TestMethod.Method.Name;

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

    #region UniqueTestName
    string GetUniqueTestName(ITestCase testCase)
    {
        var method = testCase.TestMethod;
        var name = $"{method.TestClass.Class.ClassName()}.{method.Method.Name}";
        if (!Parameters.Any())
        {
            return name;
        }

        StringBuilder builder = new($"{name}_");
        foreach (var parameter in Parameters)
        {
            builder.Append($"{parameter.Info.Name}=");
            builder.Append(string.Join(",", SplitParams(parameter.Value)));
            builder.Append('_');
        }

        builder.Length -= 1;

        return builder.ToString();
    }

    static IEnumerable<string> SplitParams(object? parameter)
    {
        if (parameter == null)
        {
            yield return "null";
            yield break;
        }

        if (parameter is string stringValue)
        {
            yield return stringValue;
            yield break;
        }

        if (parameter is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                foreach (var sub in SplitParams(item))
                {
                    yield return sub;
                }
            }

            yield break;
        }

        var toString = parameter.ToString();
        if (toString == null)
        {
            yield return "null";
        }
        else
        {
            yield return toString;
        }
    }
    #endregion
}