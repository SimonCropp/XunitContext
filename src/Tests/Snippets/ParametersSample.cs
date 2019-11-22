using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

public class ParametersSample :
    XunitContextBase
{
    [Theory]
    [MemberData(nameof(GetData))]
    public void Usage(string arg)
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

    public ParametersSample(ITestOutputHelper output) :
        base(output)
    {
    }
}