using System.Threading;

namespace XunitLogger
{
    public class IntCounter
    {
        int current;

        public int Current
        {
            get => current;
        }

        public int Next()
        {
            return Interlocked.Increment(ref current);
        }
    }
}