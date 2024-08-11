public class FilterSample(ITestOutputHelper output) :
    XunitContextBase(output)
{
    static FilterSample() =>
        Filters.Add(_ => _ != null && !_.Contains("ignored"));

    [Fact]
    public void Write_lines()
    {
        WriteLine("first");
        WriteLine("with ignored string");
        WriteLine("last");
        var logs = XunitContext.Logs;

        Assert.Contains("first", logs);
        Assert.DoesNotContain("with ignored string", logs);
        Assert.Contains("last", logs);
    }
}