using System.Threading;

namespace Xunit
{
    public class ULongCounter
    {
        long current;

        public ulong Current
        {
            get => (ulong) current;
        }

        public ulong Next()
        {
            return (ulong) Interlocked.Increment(ref current);
        }
    }
}