using System.Collections.Generic;
using System.Security.Claims;
using Xunit;
using Xunit.Abstractions;

public class NonSerializableData :
    XunitContextBase
{
    [Theory(Skip = "Will throw")]
    [MemberData(nameof(GetData))]
    public void Usage(ClaimsPrincipal arg)
    {
    }

    public static IEnumerable<object[]> GetData()
    {
        yield return new object[] {new ClaimsPrincipal()};
    }

    public NonSerializableData(ITestOutputHelper output) :
        base(output)
    {
    }
}

