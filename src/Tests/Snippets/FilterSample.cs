using Xunit;
using Xunit.Abstractions;
using XunitLogger;

public class FilterSample :
    XunitLoggingBase
{
    static FilterSample()
    {
        Filters.Items.Add(x => x != null && !x.Contains("ignored"));
    }

    [Fact]
    public void Write_lines()
    {
        WriteLine("first");
        WriteLine("with ignored string");
        WriteLine("last");
        var logs = XunitLogging.Logs;

        Assert.Contains("first", logs);
        Assert.DoesNotContain("with ignored string", logs);
        Assert.Contains("last", logs);
    }

    public FilterSample(ITestOutputHelper output) :
        base(output)
    {
    }
}