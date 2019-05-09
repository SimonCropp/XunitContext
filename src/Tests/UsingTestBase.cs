using ObjectApproval;
using Xunit;
using Xunit.Abstractions;
using XunitLogger;

public class UsingTestBase :
    XunitLoggingBase
{
    static UsingTestBase()
    {
        Filters.Items.Add(x => x != "ignored");
    }

    [Fact]
    public void Write_lines()
    {
        Write("part1");
        Write(" part2");
        WriteLine();
        WriteLine("part3");
        WriteLine("ignored");
        ObjectApprover.VerifyWithJson(Logs);
    }

    public UsingTestBase(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}