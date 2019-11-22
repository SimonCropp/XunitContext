using System.Threading;

namespace Xunit
{
    public class LongCounter
    {
        long current;

        public long Current
        {
            get => current;
        }

        public long Next()
        {
            return Interlocked.Increment(ref current);
        }
    }
}