// public class UsingTestBase(ITestOutputHelper testOutput) :
//     XunitContextBase(testOutput)
// {
//     static UsingTestBase() =>
//         Filters.Add(_ => _ != "ignored");
//
//     [Fact]
//     public Task Write_lines()
//     {
//         Write("part1");
//         Write(" part2");
//         WriteLine();
//         WriteLine("part3");
//         WriteLine("ignored");
//         return Verify(Logs);
//     }
//
//     [Fact]
//     public void CurrentTest()
//     {
//         Assert.Equal("UsingTestBase", Context.ClassName);
//         Assert.Equal("CurrentTest", Context.MethodName);
//         Assert.EndsWith("UsingTestBase.cs", Context.SourceFile);
//         Assert.True(File.Exists(Context.SourceFile));
//         Assert.EndsWith("Tests", Context.SourceDirectory);
//         Assert.True(Directory.Exists(Context.SourceDirectory));
//         Assert.EndsWith("src", Context.SolutionDirectory);
//         Assert.True(Directory.Exists(Context.SolutionDirectory));
//         Assert.EndsWith("UsingTestBase.CurrentTest", Context.UniqueTestName);
//     }
// }