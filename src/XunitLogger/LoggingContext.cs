using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace XunitLogger
{
    public partial class Context
    {
        public ITestOutputHelper TestOutput { get; internal set; }
        List<string> logMessages = new List<string>();
        object locker = new object();

        public IReadOnlyList<string> LogMessages => logMessages;

        public StringBuilder Builder;

        public void ThrowIfFlushed()
        {
            if (flushed)
            {
                throw new Exception("Logging context has been flushed.");
            }
        }

        bool flushed;

        internal Context(ITestOutputHelper testOutput)
        {
            TestOutput = testOutput;
        }

        internal Context()
        {
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
                if (Builder == null)
                {
                    logMessages.Add("");
                    if (TestOutput == null)
                    {
                        Builder = new StringBuilder();
                        Builder.AppendLine();
                        return;
                    }
                    TestOutput.WriteLine("");
                    return;
                }

                var message = Builder.ToString();
                Builder = null;
                if (Filters.ShouldFilterOut(message))
                {
                    return;
                }

                logMessages.Add(message);

                if (TestOutput == null)
                {
                    Builder = new StringBuilder(message);
                    Builder.AppendLine();
                    return;
                }
                TestOutput.WriteLine(message);
            }
        }

        public void WriteLine(string value)
        {
            Guard.AgainstNull(value, nameof(value));
            lock (locker)
            {
                ThrowIfFlushed();

                if (Builder == null && TestOutput == null)
                {
                    if (Filters.ShouldFilterOut(value))
                    {
                        return;
                    }

                    Builder = new StringBuilder();
                    Builder.AppendLine(value);
                    logMessages.Add(value);
                    return;
                }

                if (Builder != null && TestOutput != null)
                {
                    Builder.AppendLine(value);
                    var message = Builder.ToString();
                    Builder = null;
                    if (Filters.ShouldFilterOut(message))
                    {
                        return;
                    }

                    logMessages.Add(message);
                    TestOutput.WriteLine(message);
                    return;
                }

                if (Builder == null && TestOutput != null)
                {
                    if (Filters.ShouldFilterOut(value))
                    {
                        return;
                    }

                    logMessages.Add(value);
                    TestOutput.WriteLine(value);
                    return;
                }
                if (Builder != null && TestOutput == null)
                {
                    Builder.AppendLine(value);
                }
            }
        }

        public void Flush()
        {
            lock (locker)
            {
                if (flushed)
                {
                    return;
                }

                flushed = true;
                if (Builder == null)
                {
                    return;
                }

                var message = Builder.ToString();
                Builder = null;
                if (Filters.ShouldFilterOut(message))
                {
                    return;
                }

                logMessages.Add(message);
                if (TestOutput == null)
                {
                    throw new Exception("No ITestOutputHelper to flush to.");
                }
                TestOutput.WriteLine(message);
            }
        }
    }
}