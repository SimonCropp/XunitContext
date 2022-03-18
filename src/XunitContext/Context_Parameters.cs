namespace Xunit;

public partial class Context
{
    List<Parameter>? parameters;

    public IReadOnlyList<Parameter> Parameters => parameters ??= GetParameters(Test.TestCase);

    /// <summary>
    /// Override the default parameter resolution.
    /// </summary>
    public void UseParameters(params object[] parameters)
    {
        Guard.AgainstNull(parameters, nameof(parameters));
        this.parameters = GetParameters(Test.TestCase, parameters);
    }

    static List<Parameter> empty = new();

    #region Parameters
    static List<Parameter> GetParameters(ITestCase testCase) =>
        GetParameters(testCase, testCase.TestMethodArguments);

    static List<Parameter> GetParameters(ITestCase testCase, object[] arguments)
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

        List<Parameter> items = new();

        for (var index = 0; index < infos.Count; index++)
        {
            items.Add(new(infos[index], arguments[index]));
        }

        return items;
    }
    #endregion

    static Exception NewNoArgumentsDetectedException() =>
        new(@"No arguments detected for method with parameters.
This is most likely caused by using a parameter that Xunit cannot serialize.
Instead pass in a simple type as a parameter and construct the complex object inside the test.
Alternatively; override the current parameters using `UseParameters()` via the current test base class, or via `XunitContext.Current.UseParameters()`.");
}