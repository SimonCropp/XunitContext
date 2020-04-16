#pragma warning disable CS3002 // Return type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
using System;

namespace Xunit
{
    public static class Counters
    {
        static GuidCounter GuidCounter = new GuidCounter();
        static IntCounter IntCounter = new IntCounter();
        static LongCounter LongCounter = new LongCounter();
        static UIntCounter UIntCounter = new UIntCounter();
        static ULongCounter ULongCounter = new ULongCounter();

        public static uint CurrentUInt
        {
            get => UIntCounter.Current;
        }

        public static int CurrentInt
        {
            get => IntCounter.Current;
        }

        public static long CurrentLong
        {
            get => LongCounter.Current;
        }

        public static ulong CurrentULong
        {
            get => ULongCounter.Current;
        }

        public static Guid CurrentGuid
        {
            get => GuidCounter.Current;
        }

        public static uint NextUInt()
        {
            return UIntCounter.Next();
        }

        public static int NextInt()
        {
            return IntCounter.Next();
        }

        public static long NextLong()
        {
            return LongCounter.Next();
        }

        public static ulong NextULong()
        {
            return ULongCounter.Next();
        }

        public static Guid NextGuid()
        {
            return GuidCounter.Next();
        }
    }
}