using Xunit;

namespace InNamespace
{
    [GlobalSetUp]
    public static class GlobalSetup
    {
        public static void Setup()
        {
            Called = true;
        }

        public static bool Called;
    }
}