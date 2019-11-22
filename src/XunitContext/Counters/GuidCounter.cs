using System;
using System.Threading;

namespace XunitLogger
{
    public class GuidCounter
    {
        int current;

        public Guid Current
        {
            get => IntToGuid(current);
        }

        public Guid Next()
        {
            var value = Interlocked.Increment(ref current);
            return IntToGuid(value);
        }

        static Guid IntToGuid(int value)
        {
            var bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}