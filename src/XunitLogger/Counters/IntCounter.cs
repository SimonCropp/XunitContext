using System.Threading;

namespace XunitLogger
{
    public class IntCounter
    {
        int seed;

        public int Next()
        {
            return Interlocked.Increment(ref seed);
        }
    }
}