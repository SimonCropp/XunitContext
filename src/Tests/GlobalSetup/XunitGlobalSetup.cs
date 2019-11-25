public static class XunitGlobalSetup
{
    public static void Setup()
    {
        Called = true;
    }

    public static bool Called;
}