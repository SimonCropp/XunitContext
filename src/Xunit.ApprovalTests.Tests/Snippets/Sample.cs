using ApprovalTests;
using ApprovalTests.Namers;
using Xunit;
using Xunit.Abstractions;

public class Sample :
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

    [Theory]
    [InlineData("Foo")]
    [InlineData(9)]
    [InlineData(true)]
    public void Theory(object value)
    {
        Approvals.Verify(value);
    }

    public Sample(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}