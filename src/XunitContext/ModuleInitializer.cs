using Xunit;

static class ModuleInitializer
{
    public static void Initialize()
    {
        XunitContext.Init();
    }
}