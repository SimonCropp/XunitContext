using ApprovalTests;
using Xunit;
using Xunit.Abstractions;

public class UsingTestBase :
    XunitApprovalBase
{
    [Fact]
    public void Simple()
    {
        Approvals.Verify("SimpleResult");
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
    [InlineData("Foo", "Bar")]
    [InlineData(9, false)]
    [InlineData(true, -1)]
    public void MultiTheory(object value1, object value2)
    {
        Approvals.Verify($"{value1} {value2}");
    }

    public UsingTestBase(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}