using System;
using System.Threading;

namespace XunitLogger
{
    public class GuidCounter
    {
        int seed;

        public Guid Next()
        {
            var value = Interlocked.Increment(ref seed);
            var bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}