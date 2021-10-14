using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

public class ComplexParameterSample :
    XunitContextBase
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void UseComplexMemberData(ComplexClass arg)
    {
        UseParameters(arg);
        var parameter = Context.Parameters.Single();
        var parameterInfo = parameter.Info;
        Assert.Equal("arg", parameterInfo.Name);
        Assert.Equal(arg, parameter.Value);
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[] {new ComplexClass("Value1")};
        yield return new object[] {new ComplexClass("Value2")};
    }

    public ComplexParameterSample(ITestOutputHelper output) :
        base(output)
    {
    }

    public class ComplexClass
    {
        public string Value { get; }

        public ComplexClass(string value)
        {
            Value = value;
        }
    }
}