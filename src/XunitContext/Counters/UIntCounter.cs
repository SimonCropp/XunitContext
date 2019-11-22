using System.Threading;

namespace Xunit
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