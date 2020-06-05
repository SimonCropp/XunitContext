using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit.Abstractions;

namespace Xunit
{
    public partial class Context
    {
        /// <summary>
        /// The current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public ITestOutputHelper? TestOutput { get; internal set; }
        /// <summary>
        /// The source file that the current test exists in.
        /// </summary>
        public string SourceFile { get; internal set; } = null!;
        /// <summary>
        /// The source directory that the current test exists in.
        /// </summary>
        public string SourceDirectory
        {
            get => Path.GetDirectoryName(SourceFile);
        }

        List<string> logMessages = new List<string>();
        object locker = new object();

        /// <summary>
        /// All log message that have been written to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public IReadOnlyList<string> LogMessages => logMessages;

        public StringBuilder? Builder;

        public void ThrowIfFlushed(string logText)
        {
            if (flushed)
            {
                throw new Exception($"Context has been flushed. Could not write the string: {logText}");
            }
        }

        internal bool flushed;

        internal Context(ITestOutputHelper testOutput, string sourceFile)
        {
            TestOutput = testOutput;
            SourceFile = sourceFile;
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

        /// <summary>
        /// Writes a value to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void Write(object value)
        {
            Guard.AgainstNull(value, nameof(value));
            Write(value.ToString());
        }

        /// <summary>
        /// Writes a string to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void Write(string value)
        {
            Guard.AgainstNull(value, nameof(value));
            lock (locker)
            {
                ThrowIfFlushed(value);
                InitBuilder();
                Builder?.Append(value);
            }
        }

        /// <summary>
        /// Writes a <see cref="char"/> to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void Write(char value)
        {
            lock (locker)
            {
                ThrowIfFlushed(value.ToString());
                InitBuilder();
                Builder?.Append(value);
            }
        }

        /// <summary>
        /// Writes a line to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void WriteLine()
        {
            lock (locker)
            {
                ThrowIfFlushed("Environment.NewLine");

                if (Builder == null && TestOutput == null)
                {
                    Builder = new StringBuilder();
                    Builder.AppendLine();
                    logMessages.Add(string.Empty);
                    return;
                }

                if (Builder != null && TestOutput != null)
                {
                    var message = Builder.ToString();
                    Builder = null;
                    logMessages.Add(message);
                    TestOutput.WriteLine(message);
                    return;
                }

                if (Builder == null && TestOutput != null)
                {
                    logMessages.Add(string.Empty);
                    TestOutput.WriteLine(string.Empty);
                    return;
                }
                if (Builder != null && TestOutput == null)
                {
                    Builder.AppendLine();
                }
            }
        }

        /// <summary>
        /// Writes a line to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void WriteLine(object value)
        {
            Guard.AgainstNull(value, nameof(value));
            WriteLine(value.ToString());
        }

        /// <summary>
        /// Writes a line to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void WriteLine(string value)
        {
            Guard.AgainstNull(value, nameof(value));
            lock (locker)
            {
                ThrowIfFlushed(value);

                if (Builder == null && TestOutput == null)
                {
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
                    logMessages.Add(message);
                    TestOutput.WriteLine(message);
                    return;
                }

                if (Builder == null && TestOutput != null)
                {
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