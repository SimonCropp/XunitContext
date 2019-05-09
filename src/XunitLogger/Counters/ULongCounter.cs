using System.Threading;

namespace XunitLogger
{
    public class ULongCounter
    {
        long seed;

        public ulong Next()
        {
            return (ulong) Interlocked.Increment(ref seed);
        }
    }
}