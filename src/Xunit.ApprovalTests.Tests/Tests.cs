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
        using (NamerFactory.AsEnvironmentSpecificTest(() => "Foo"))
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
    public void Theory(object value)
    {
        Approvals.Verify(value);
    }

    [Theory]
    [InlineData("Foo")]
    public void TheoryAsEnvironmentSpecificTest(object value)
    {
        using (NamerFactory.AsEnvironmentSpecificTest(() => "Bar"))
        {
            Approvals.Verify(value);
        }
    }

    [Theory]
    [InlineData("Foo", "Bar")]
    [InlineData(9, false)]
    [InlineData(true, -1)]
    public void MultiTheory(object value1, object value2)
    {
        Approvals.Verify($"{value1} {value2}");
    }

    public Tests(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}