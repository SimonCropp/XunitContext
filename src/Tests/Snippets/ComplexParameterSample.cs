public class ComplexParameterSample(ITestOutputHelper output) :
    XunitContextBase(output)
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
        yield return [new ComplexClass("Value1")];
        yield return [new ComplexClass("Value2")];
    }

    public class ComplexClass(string value)
    {
        public string Value { get; } = value;
    }
}