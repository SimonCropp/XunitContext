public class ParametersTests(ITestOutputHelper output) :
    XunitContextBase(output)
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
        Assert.Equal($"ParametersTests.InlineData_arg={arg}", Context.UniqueTestName);
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void MemberData(string arg)
    {
        var parameter = Context.Parameters.Single();
        var parameterInfo = parameter.Info;
        Assert.Equal("arg", parameterInfo.Name);
        Assert.Equal(arg, parameter.Value);
        Assert.Equal($"ParametersTests.MemberData_arg={arg}", Context.UniqueTestName);
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[]
        {
            "Value1"
        };
        yield return new object[]
        {
            "Value2"
        };
    }

    [Theory]
    [MemberData(nameof(GetEnumerableData))]
    public void EnumerableMemberData(string arg1, string[] arg2) =>
        Assert.Equal("ParametersTests.EnumerableMemberData_arg1=Value1_arg2=Value2,Value3,null", Context.UniqueTestName);

    public static IEnumerable<object?[]> GetEnumerableData()
    {
        yield return new object?[]
        {
            "Value1",
            new[]
            {
                "Value2",
                "Value3",
                null
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetDataComplex))]
    public void ShouldNotThrowIfParamsNotAccessed(ComplexClass arg)
    {
    }

    [Theory]
    [MemberData(nameof(GetDataComplex))]
    public async Task ShouldThrowForComplexParam(ComplexClass arg)
    {
        var exception = Assert.Throws<Exception>(() =>
        {
            // ReSharper disable once UnusedVariable
            var parameter = Context.Parameters.Single();
        });
        var md = Path.Combine(SourceDirectory, "NoArgumentsDetectedException.include.md");
        File.Delete(md);
        using var writer = File.CreateText(md);
        await writer.WriteLineAsync();
        foreach (var line in exception.Message.Split(new[]
                 {
                     '\r',
                     '\n'
                 }, StringSplitOptions.RemoveEmptyEntries))
        {
            await writer.WriteLineAsync($"> {line}");
        }

        await writer.WriteLineAsync();
    }

    [Theory]
    [MemberData(nameof(GetDataComplex))]
    public void MemberDataComplex(ComplexClass arg)
    {
        UseParameters(arg);
        var parameter = Context.Parameters.Single();
        var parameterInfo = parameter.Info;
        Assert.Equal("arg", parameterInfo.Name);
        Assert.Equal(arg, parameter.Value);
    }

    public static IEnumerable<object[]> GetDataComplex()
    {
        yield return new object[]
        {
            new ComplexClass("Value1")
        };
        yield return new object[]
        {
            new ComplexClass("Value2")
        };
    }
}

public class ComplexClass(string value)
{
    public string Value { get; } = value;
}