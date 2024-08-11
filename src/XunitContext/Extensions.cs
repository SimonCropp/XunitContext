using System.Runtime.InteropServices.ComTypes;

static class Extensions
{
    public static string TrimTrailingNewline(this string value) =>
        value[..^Environment.NewLine.Length];

    public static MethodBase GetRealMethod(this MethodBase method)
    {
        var declaringType = method.DeclaringType!;
        if (!typeof(IAsyncStateMachine).IsAssignableFrom(declaringType))
        {
            return method;
        }

        var realType = declaringType.DeclaringType!;
        foreach (var methodInfo in realType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var stateMachineAttribute = methodInfo.GetCustomAttribute<AsyncStateMachineAttribute>();
            if (stateMachineAttribute == null)
            {
                continue;
            }

            if (stateMachineAttribute.StateMachineType == declaringType)
            {
                return methodInfo;
            }
        }

        return method;
    }

    public static IXunitTestMethod TestMethod(this ITest test)
    {
        var testMethod = test.TestCase.TestMethod;
        if (testMethod == null)
        {
            throw new("TestCase.TestMethod is null");
        }

        if (testMethod is IXunitTestMethod xunitTestMethod)
        {
            return xunitTestMethod;
        }

        throw new("TestCase.TestMethod is not IXunitTestMethod");
    }

    public static IXunitTestClass TestClass(this ITest test)
    {
        var baseTestClass = test.TestCase.TestClass;
        if (baseTestClass == null)
        {
            throw new("TestContext.TestClass is null");
        }

        if (baseTestClass is IXunitTestClass xunitTestClass)
        {
            return xunitTestClass;
        }

        throw new("TestCase.TestClass is not IXunitTestClass");
    }

    public static string ClassName(this ITypeInfo value)
    {
        var name = value.Name;
        var indexOf = name.LastIndexOf('.');
        if (indexOf == -1)
        {
            return name;
        }

        return name.Substring(indexOf + 1, name.Length - indexOf - 1);
    }
}