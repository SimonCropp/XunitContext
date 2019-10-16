using ApprovalTests;
using Xunit;
using Xunit.Abstractions;

namespace TheNamespace
{
    public class WithNamespace :
        XunitApprovalBase
    {
        [Fact]
        public void Simple()
        {
            Approvals.Verify("SimpleResult");
        }

        public WithNamespace(ITestOutputHelper output) :
            base(output)
        {
        }
    }
}