using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace XunitLogger
{
    public class Context
    {
        ITestOutputHelper TestOutput;
        GuidCounter GuidCounter = new GuidCounter();
        IntCounter IntCounter = new IntCounter();
        LongCounter LongCounter = new LongCounter();
        UIntCounter UIntCounter = new UIntCounter();
        ULongCounter ULongCounter = new ULongCounter();
        List<string> logMessages = new List<string>();
        object locker = new object();

        public IReadOnlyList<string> LogMessages
        {
            get => logMessages;
        }

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

        public StringBuilder Builder;

        public void ThrowIfFlushed()
        {
            if (Flushed)
            {
                throw new Exception("Logging context has been flushed.");
            }
        }

        public bool Flushed;

        internal Context(ITestOutputHelper testOutput)
        {
            TestOutput = testOutput;
        }

        void InitBuilder()
        {
            if (Builder == null)
            {
                Builder = new StringBuilder();
            }
        }

        public void Write(string value)
        {
            Guard.AgainstNull(value, nameof(value));
            lock (locker)
            {
                ThrowIfFlushed();
                InitBuilder();
                Builder.Append(value);
            }
        }

        public void Write(char value)
        {
            lock (locker)
            {
                ThrowIfFlushed();
                InitBuilder();
                Builder.Append(value);
            }
        }

        public void WriteLine()
        {
            lock (locker)
            {
                ThrowIfFlushed();
                var builder = Builder;
                if (Builder == null)
                {
                    logMessages.Add("");
                    TestOutput.WriteLine("");
                    return;
                }

                var message = builder.ToString();
                Builder = null;
                if (Filters.ShouldFilterOut(message))
                {
                    return;
                }

                logMessages.Add(message);
                TestOutput.WriteLine(message);
            }
        }

        public void WriteLine(string value)
        {
            Guard.AgainstNull(value, nameof(value));
            lock (locker)
            {
                ThrowIfFlushed();
                var builder = Builder;
                if (Builder == null)
                {
                    if (Filters.ShouldFilterOut(value))
                    {
                        return;
                    }

                    logMessages.Add(value);
                    TestOutput.WriteLine(value);
                    return;
                }

                builder.Append(value);
                var message = builder.ToString();
                Builder = null;
                if (Filters.ShouldFilterOut(message))
                {
                    return;
                }

                logMessages.Add(message);
                TestOutput.WriteLine(message);
            }
        }

        public void Flush()
        {
            lock (locker)
            {
                if (Flushed)
                {
                    return;
                }

                Flushed = true;
                var builder = Builder;
                if (builder == null)
                {
                    return;
                }

                var message = builder.ToString();
                Builder = null;
                if (Filters.ShouldFilterOut(message))
                {
                    return;
                }

                logMessages.Add(message);
                TestOutput.WriteLine(message);
            }
        }
    }
}