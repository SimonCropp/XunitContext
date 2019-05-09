using System;
using System.Collections.Concurrent;

namespace XunitLogger
{
    public static class Filters
    {
        //TODO: wrap in add method and guards
        public static ConcurrentBag<Func<string, bool>> Items = new ConcurrentBag<Func<string, bool>>();

        internal static bool ShouldFilterOut(string message)
        {
            foreach (var filter in Items)
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