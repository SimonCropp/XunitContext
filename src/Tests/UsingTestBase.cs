using System;
using ObjectApproval;
using Xunit;
using Xunit.Abstractions;

public class UsingTestBase :
    XunitLoggingBase
{
    [Fact]
    public void Write_lines()
    {
        Write("part1");
        Write(" part2");
        WriteLine();
        WriteLine("part3");
        ObjectApprover.VerifyWithJson(Logs);
    }

    public UsingTestBase(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}