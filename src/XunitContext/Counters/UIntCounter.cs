using System.Threading;

namespace XunitLogger
{
    public class UIntCounter
    {
        int current;

        public uint Current
        {
            get => (uint) current;
        }

        public uint Next()
        {
            return (uint) Interlocked.Increment(ref current);
        }
    }
}