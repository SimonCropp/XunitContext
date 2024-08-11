public class UsingClassFixture(ITestOutputHelper helper, ContextFixture fixture) :
    IContextFixture
{
    static UsingClassFixture() =>
        Filters.Add(_ => _ != "ignored");

    Context context = fixture.Start(helper);

    [Fact]
    public void CurrentTest()
    {
        Assert.Equal("UsingClassFixture", context.ClassName);
        Assert.Equal("CurrentTest", context.MethodName);
        Assert.EndsWith("UsingClassFixture.cs", context.SourceFile);
        Assert.True(File.Exists(context.SourceFile));
        Assert.EndsWith("Tests", context.SourceDirectory);
        Assert.True(Directory.Exists(context.SourceDirectory));
        Assert.EndsWith("src", context.SolutionDirectory);
        Assert.True(Directory.Exists(context.SolutionDirectory));
        Assert.EndsWith("UsingClassFixture.CurrentTest", context.UniqueTestName);
    }

    [Fact]
    public Task Write_lines()
    {
        Debug.Write("part1");
        Console.Write(" part2");
        Console.WriteLine();
        Console.WriteLine("part3");
        Console.WriteLine("ignored");
        return Verify(context.LogMessages);
    }
}