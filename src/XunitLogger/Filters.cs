using System;
using System.Collections.Concurrent;

namespace XunitLogger
{
    public static class Filters
    {
        static ConcurrentBag<Func<string, bool>> items = new ConcurrentBag<Func<string, bool>>();

        public static void Add(Func<string, bool> filter)
        {
            Guard.AgainstNull(filter, nameof(filter));
            items.Add(filter);
        }

        public static void Clear()
        {
            Func<string, bool> someItem;
            while (!items.IsEmpty)
            {
                items.TryTake(out someItem);
            }
        }

        internal static bool ShouldFilterOut(string message)
        {
            foreach (var filter in items)
            {
                if (!filter(message))
                {
                    return true;
                }
            }

            return false;
        }
    }
}