using System.Text;
using TextCopy;
using Xunit;
using Xunit.Abstractions;

public class PerformanceTestBuilder :
    XunitApprovalBase
{
    [Fact]
    public void Baseline()
    {
        var builder = new StringBuilder();
        builder.AppendLine(@"using System.Threading.Tasks;
using ApprovalTests;
using Xunit;
using Xunit.Abstractions;
");
        for (var classIndex = 0; classIndex < 10; classIndex++)
        {
            builder.AppendLine($@"
public class TestClass{classIndex}
{{");
            for (var methodIndex = 0; methodIndex < 10; methodIndex++)
            {
                builder.AppendLine($@"
    [Fact]
    public void Test{methodIndex}()
    {{
        Approvals.Verify(""SimpleResult"");
    }}

    [Fact]
    public async Task TestAsync{methodIndex}()
    {{
        await Task.Delay(0);
        Approvals.Verify(""SimpleResult"");
    }}
");
            }

            builder.AppendLine($@"
    public TestClass{classIndex}(ITestOutputHelper output)
    {{
    }}
}}");
        }

        Clipboard.SetText(builder.ToString());
    }

    [Fact]
    public void Simple()
    {
        var builder = new StringBuilder();
        builder.AppendLine(@"using System.Threading.Tasks;
using ApprovalTests;
using Xunit;
using Xunit.Abstractions;
");
        for (var classIndex = 0; classIndex < 10; classIndex++)
        {
            builder.AppendLine($@"
public class TestClass{classIndex} :
    XunitApprovalBase
{{");
            for (var methodIndex = 0; methodIndex < 10; methodIndex++)
            {
                builder.AppendLine($@"
    [Fact]
    public void Test{methodIndex}()
    {{
        Approvals.Verify(""SimpleResult"");
    }}

    [Fact]
    public async Task TestAsync{methodIndex}()
    {{
        await Task.Delay(0);
        Approvals.Verify(""SimpleResult"");
    }}
");
            }

            builder.AppendLine($@"
    public TestClass{classIndex}(ITestOutputHelper output) :
        base(output)
    {{
    }}
}}");
        }

        Clipboard.SetText(builder.ToString());
    }

    public PerformanceTestBuilder(ITestOutputHelper testOutput) :
        base(testOutput)
    {
    }
}