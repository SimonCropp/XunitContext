using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            var parameter = Context.Parameters.Single();
        });
        var md = Path.Combine(SourceDirectory, "NoArgumentsDetectedException.include.md");
        File.Delete(md);
        using var writer = File.CreateText(md);
        await writer.WriteLineAsync();
        foreach (var line in exception.Message.Split(new[]{ '\r', '\n'},StringSplitOptions.RemoveEmptyEntries))
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
        yield return new object[] {new ComplexClass("Value1")};
        yield return new object[] {new ComplexClass("Value2")};
    }

    public ParametersTests(ITestOutputHelper output) :
        base(output)
    {
    }
}

public class ComplexClass
{
    public string Value { get; }

    public ComplexClass(string value)
    {
        Value = value;
    }
}