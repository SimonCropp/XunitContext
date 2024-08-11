using Xunit.v3;

namespace XunitV3Context;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class XunitContextAttribute :
    BeforeAfterTestAttribute
{
    //static AsyncLocal<MethodInfo?> local = new();

    public override ValueTask Before(MethodInfo info, IXunitTest test)
    {
        return default;
    }

    public override ValueTask After(MethodInfo info, IXunitTest test)
    {
        return default;
    }

    // internal static bool TryGet([NotNullWhen(true)] out MethodInfo? info)
    // {
    //     info = local.Value;
    //     return info is not null;
    // }
}