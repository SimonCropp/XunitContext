using System.Threading;

namespace Xunit
{
    public abstract class Counter<T>
    {
        int current;

        protected abstract T Convert(int i);

        public T Current
        {
            get => Convert(current);
        }

        public T Next()
        {
            var increment = Interlocked.Increment(ref current);

            return Convert(increment);
        }
    }
}