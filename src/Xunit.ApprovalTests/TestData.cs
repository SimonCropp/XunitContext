using System;
using System.Reflection;

class TestData
{
    Type testType;
    MethodInfo testMethod;

    public TestData(Type testType, MethodInfo testMethod)
    {
        this.testType = testType;
        this.testMethod = testMethod;
    }

    public T? GetFirstFrameForAttribute<T>()
        where T : Attribute
    {
        var attribute = testMethod.GetCustomAttribute<T>();
        if (attribute != null)
        {
            return attribute;
        }

        attribute = testType.GetCustomAttribute<T>(true);
        if (attribute != null)
        {
            return attribute;
        }

        attribute = testType.Assembly.GetCustomAttribute<T>();
        if (attribute != null)
        {
            return attribute;
        }

        return null;
    }
}