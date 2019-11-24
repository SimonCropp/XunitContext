using System;

namespace Xunit
{
    public partial class Context
    {
        GuidCounter GuidCounter = new GuidCounter();
        IntCounter IntCounter = new IntCounter();
        LongCounter LongCounter = new LongCounter();
        UIntCounter UIntCounter = new UIntCounter();
        ULongCounter ULongCounter = new ULongCounter();

        public uint CurrentUInt
        {
            get => UIntCounter.Current;
        }

        public int CurrentInt
        {
            get => IntCounter.Current;
        }

        public long CurrentLong
        {
            get => LongCounter.Current;
        }

        public ulong CurrentULong
        {
            get => ULongCounter.Current;
        }

        public Guid CurrentGuid
        {
            get => GuidCounter.Current;
        }

        public uint NextUInt(uint input)
        {
            return UIntCounter.Next(input);
        }

        public T Next<T>(T input)
        {
            if (input is Guid guidInput)
            {
                return (T) (object) GuidCounter.Next(guidInput);
            }

            if (input is int intInput)
            {
                return (T) (object) IntCounter.Next(intInput);
            }

            if (input is uint uIntInput)
            {
                return (T) (object) UIntCounter.Next(uIntInput);
            }

            if (input is ulong ulongInput)
            {
                return (T) (object) ULongCounter.Next(ulongInput);
            }

            if (input is long longInput)
            {
                return (T) (object) LongCounter.Next(longInput);
            }

            throw new Exception($"Unknown type {typeof(T).FullName}");
        }

        public int NextInt(int input)
        {
            return IntCounter.Next(input);
        }

        public long NextLong(long input)
        {
            return LongCounter.Next(input);
        }

        public ulong NextULong(ulong input)
        {
            return ULongCounter.Next(input);
        }

        public Guid NextGuid(Guid input)
        {
            return GuidCounter.Next(input);
        }

        public uint NextUInt()
        {
            return UIntCounter.Next();
        }

        public int NextInt()
        {
            return IntCounter.Next();
        }

        public long NextLong()
        {
            return LongCounter.Next();
        }

        public ulong NextULong()
        {
            return ULongCounter.Next();
        }

        public Guid NextGuid()
        {
            return GuidCounter.Next();
        }
    }
}