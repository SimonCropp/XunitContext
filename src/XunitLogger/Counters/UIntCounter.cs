using System.Threading;

namespace XunitLogger
{
    public class UIntCounter
    {
        int seed;

        public uint Next()
        {
            return (uint) Interlocked.Increment(ref seed);
        }
    }
}