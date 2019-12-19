using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

public class ParametersTests :
    XunitContextBase
{
    [Theory]
    [InlineData("Value1")]
    [InlineData("Value2")]
    public void InlineData(string arg)
    {
        var parameter = Context.Parameters.Single();
        var parameterInfo = parameter.Info;
        Assert.Equal("arg", parameterInfo.Name);
        Assert.Equal(arg, parameter.Value);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void MemberData(string arg)
    {
        var parameter = Context.Parameters.Single();
        var parameterInfo = parameter.Info;
        Assert.Equal("arg", parameterInfo.Name);
        Assert.Equal(arg, parameter.Value);
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[] {"Value1"};
        yield return new object[] {"Value2"};
    }

    public ParametersTests(ITestOutputHelper output) :
        base(output)
    {
    }
}