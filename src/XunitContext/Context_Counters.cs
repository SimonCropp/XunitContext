#pragma warning disable CS3002 // Return type is not CLS-compliant
#pragma warning disable CS3003 // Type is not CLS-compliant
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
        DateTimeOffsetCounter DateTimeOffsetCounter = new DateTimeOffsetCounter();
        DateTimeCounter DateTimeCounter = new DateTimeCounter();

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

        public DateTime CurrentDateTime
        {
            get => DateTimeCounter.Current;
        }

        public DateTimeOffset CurrentDateTimeOffset
        {
            get => DateTimeOffsetCounter.Current;
        }

        public int IntOrNext<T>(T input)
        {
            if (input is Guid guidInput)
            {
                return GuidCounter.IntOrNext(guidInput);
            }

            if (input is int intInput)
            {
                return IntCounter.IntOrNext(intInput);
            }

            if (input is uint uIntInput)
            {
                return UIntCounter.IntOrNext(uIntInput);
            }

            if (input is ulong ulongInput)
            {
                return ULongCounter.IntOrNext(ulongInput);
            }

            if (input is long longInput)
            {
                return LongCounter.IntOrNext(longInput);
            }

            if (input is DateTime dateTimeInput)
            {
                return DateTimeCounter.IntOrNext(dateTimeInput);
            }

            if (input is DateTimeOffset dateTimeOffsetInput)
            {
                return DateTimeOffsetCounter.IntOrNext(dateTimeOffsetInput);
            }

            throw new Exception($"Unknown type {typeof(T).FullName}");
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

        public DateTime NextDateTime()
        {
            return DateTimeCounter.Next();
        }

        public DateTimeOffset NextTimeOffset()
        {
            return DateTimeOffsetCounter.Next();
        }
    }
}