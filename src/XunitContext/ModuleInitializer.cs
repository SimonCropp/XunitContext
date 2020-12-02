using System.Runtime.CompilerServices;
using Xunit;

static class ModuleInit
{
    [ModuleInitializer]
    public static void Initialize()
    {
        XunitContext.Init();
    }
}