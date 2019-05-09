using System.Threading;

namespace XunitLogger
{
    public class LongCounter
    {
        long seed;

        public long Next()
        {
            return Interlocked.Increment(ref seed);
        }
    }
}