using System.Collections.Generic;
using ApprovalTests;
using ApprovalTests.Namers;
using Xunit;
using Xunit.Abstractions;

public class Tests :
    XunitApprovalBase
{
    [Fact]
    public void Simple()
    {
        Approvals.Verify("SimpleResult");
    }

    [Fact]
    [UseApprovalSubdirectory("SubDir")]
    public void InSubDir()
    {
        Approvals.Verify("SimpleResult");
    }

    [Fact]
    public void AsEnvironmentSpecificTest()
    {
        using (NamerFactory.AsEnvironmentSpecificTest("Foo"))
        {
            Approvals.Verify("Value");
        }
    }

    [Fact]
    public void ForScenarioTest()
    {
        using (ApprovalResults.ForScenario("Scenario"))
        {
            Approvals.Verify("ScenarioValue");
        }
    }

    [Theory]
    [InlineData("Foo")]
    [InlineData(9)]
    [InlineData(true)]
    public void Theory(object arg)
    {
        Approvals.Verify(arg);
    }

    [Theory]
    [InlineData("Foo")]
    public void TheoryAsEnvironmentSpecificTest(object arg)
    {
        using (NamerFactory.AsEnvironmentSpecificTest("Bar"))
        {
            Approvals.Verify(arg);
        }
    }

    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData(9, false)]
    [InlineData(true, -1)]
    public void MultiTheory(object arg1, object arg2)
    {
        Approvals.Verify($"{arg1} {arg2}");
    }

    [Theory]
    [MemberData(nameof(GetData))]
    public void MemberDataTheory(string arg)
    {
        Approvals.Verify(arg);
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[] {"Value1"};
        yield return new object[] {"Value2"};
    }

    [Theory]
    [MemberData(nameof(MultiGetData))]
    public void MultiMemberDataTheory(string arg1, string arg2)
    {
        Approvals.Verify($"{arg1} {arg2}");
    }

    public static IEnumerable<object[]> MultiGetData()
    {
        yield return new object[] {"Value1A", "Value1B"};
        yield return new object[] {"Value2A", "Value2B"};
    }

    public Tests(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}