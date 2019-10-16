using Xunit;
using Xunit.Abstractions;
using XunitLogger;

public class UsingTestBase :
    XunitLoggingBase
{
    static UsingTestBase()
    {
        Filters.Add(x => x != "ignored");
    }

    [Fact]
    public void Write_lines()
    {
        Write("part1");
        Write(" part2");
        WriteLine();
        WriteLine("part3");
        WriteLine("ignored");
        ObjectApprover.Verify(Logs);
    }

    [Fact]
    public void CurrentTest()
    {
        Assert.Equal("UsingTestBase.CurrentTest", Context.Test.DisplayName);
        Assert.Equal("UsingTestBase", Context.ClassName);
        Assert.Equal("CurrentTest", Context.MethodName);
        Assert.EndsWith("UsingTestBase.cs", Context.SourceFile);
        Assert.EndsWith("UsingTestBase.CurrentTest", Context.UniqueTestName);
    }

    public UsingTestBase(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}