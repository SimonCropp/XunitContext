using System.Threading.Tasks;
using Verify;
using Xunit;

static class Verifier
{
    public static Task Verify<T>(
        T target,
        VerifySettings? settings = null)
    {
        return GetVerifier().Verify(target, settings);
    }

    static InnerVerifier GetVerifier()
    {
        var context = XunitContext.Context;
        return new InnerVerifier(context.TestType, context.SourceDirectory, context.UniqueTestName);
    }
}