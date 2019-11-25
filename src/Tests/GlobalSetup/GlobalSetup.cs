using Xunit;

[GlobalSetUp]
public static class GlobalSetup
{
    public static void Setup()
    {
        Called = true;
    }

    public static bool Called;
}
