using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;
using Xunit.Sdk;

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
        string? solutionDirectory;

        /// <summary>
        /// The current solution directory. Obtained by walking up the directory tree from <see cref="SourceDirectory"/>.
        /// </summary>
        public string SolutionDirectory
        {
            get => solutionDirectory ??= SolutionDirectoryFinder.Find(SourceDirectory);
        }

        List<string> logMessages = new List<string>();
        object locker = new object();

        /// <summary>
        /// All log message that have been written to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public IReadOnlyList<string> LogMessages => logMessages;
        internal Exception? Exception;

        /// <summary>
        /// The <see cref="Exception"/> for the current test if it failed.
        /// </summary>
        public Exception? TestException
        {
            get
            {
                if (Exception == null)
                {
                    return null;
                }

                if (Exception is XunitException)
                {
                    return Exception;
                }
                var outerTrace = new StackTrace(Exception, false);
                var firstFrame = outerTrace.GetFrame(outerTrace.FrameCount - 1);
                var firstMethod = firstFrame.GetMethod();

                var root = firstMethod.DeclaringType.DeclaringType;
                if (root != null && root == typeof(ExceptionAggregator))
                {
                    if (Exception is TargetInvocationException targetInvocationException)
                    {
                        return targetInvocationException.InnerException;
                    }
                    return Exception;
                }

                return null;
            }
        }

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
            Builder ??= new StringBuilder();
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

                // Split on '\n'
                var start = 0;
                do
                {
                    var pos = value.IndexOf('\n', start);
                    if (pos < 0)
                    {
                        // No more '\n' characters.
                        break;
                    }

                    if (pos < 1)
                    {
                        // Trim any trailing '\r' in Builder
                        var end = (Builder?.Length ?? 0) - 1;
                        if (end > -1 && Builder![Builder.Length - 1] == '\r')
                        {
                            Builder.Remove(end, 1);
                        }
                    }
                    var count = (pos > start && value[pos - 1] == '\r' ? pos - 1 : pos) - start;

                    WriteLine(count > 0 ? value.Substring(start, count) : string.Empty);
                    start = pos + 1;
                } while (start < value.Length);

                if (start >= value.Length)
                    return;

                InitBuilder();
                Builder?.Append(start > 0 ? value.Substring(start) : value);
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
                if (value == '\n')
                {
                    // Trim any trailing '\r'
                    var end = (Builder?.Length ?? 0) - 1;
                    if (end > -1 && Builder![Builder.Length - 1] == '\r')
                    {
                        Builder.Remove(end, 1);
                    }
                    WriteLine();
                    return;
                }
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
                    Builder.Append(value);
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