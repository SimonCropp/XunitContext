using ApprovalTests;
using ApprovalTests.Namers;
using Xunit;
using Xunit.Abstractions;

#region XunitApprovalBaseUsage
public class Sample :
    XunitApprovalBase
{
    [Fact]
    public void Simple()
    {
        Approvals.Verify("SimpleResult");
    }

    public Sample(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
    #endregion

    #region UseApprovalSubdirectory
    [Fact]
    [UseApprovalSubdirectory("SubDir")]
    public void InSubDir()
    {
        Approvals.Verify("SimpleResult");
    }
    #endregion

    #region AsEnvironmentSpecificTest
    [Fact]
    public void AsEnvironmentSpecificTest()
    {
        using (NamerFactory.AsEnvironmentSpecificTest("Foo"))
        {
            Approvals.Verify("Value");
        }
    }
    #endregion

    #region ForScenario
    [Fact]
    public void ForScenarioTest()
    {
        using (ApprovalResults.ForScenario("Name"))
        {
            Approvals.Verify("Value");
        }
    }
    #endregion

    #region Theory
    [Theory]
    [InlineData("Foo")]
    [InlineData(9)]
    [InlineData(true)]
    public void Theory(object value)
    {
        Approvals.Verify(value);
    }
    #endregion
}