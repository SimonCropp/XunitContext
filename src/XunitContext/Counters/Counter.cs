using System.Collections.Generic;
using System.Threading;

namespace Xunit
{
    public abstract class Counter<T>
    {
        Dictionary<T, int> cache = new Dictionary<T, int>();
        int current;

        protected abstract T Convert(int i);

        public T Current
        {
            get => Convert(current);
        }

        public T Next(T input)
        {
            if (cache.TryGetValue(input, out var cached))
            {
                return Convert(cached);
            }
            return Next();
        }

        public T Next()
        {
            var increment = Interlocked.Increment(ref current);

            var convert = Convert(increment);
            cache[convert] = increment;
            return convert;
        }
    }
}