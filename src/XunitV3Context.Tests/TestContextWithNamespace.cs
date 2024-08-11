namespace TheNamespace;

public class TestContextWithNamespace(ITestOutputHelper testOutput) :
    XunitContextBase(testOutput)
{
    [Fact]
    public void CurrentTest()
    {
       // Assert.Equal("TestContextWithNamespace", Context.ClassName);
        //Assert.Equal("CurrentTest", Context.MethodName);
       // Assert.EndsWith("TestContextWithNamespace.cs", Context.SourceFile);
       // Assert.EndsWith("TestContextWithNamespace.CurrentTest", Context.UniqueTestName);
    }
}