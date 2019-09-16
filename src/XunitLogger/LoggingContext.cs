using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Xunit.Abstractions;

namespace XunitLogger
{
    public partial class Context
    {
        public ITestOutputHelper? TestOutput { get; internal set; }
        public string? SourceFilePath { get; internal set; }
        List<string> logMessages = new List<string>();
        object locker = new object();

        public IReadOnlyList<string> LogMessages => logMessages;
        internal Exception? Exception;

        public Exception? TestException
        {
            get
            {
                if (Exception?.InnerException == null)
                {
                    return null;
                }
                var stackTrace = new StackTrace(Exception?.InnerException,false);
                var stackFrame = stackTrace.GetFrame(stackTrace.FrameCount-1);
                var methodBase = stackFrame.GetMethod();
                var declaringType = methodBase.DeclaringType;
                var testMethod = Test.TestCase.TestMethod;
                if (testMethod.Method.Name != methodBase.Name)
                {
                    return null;
                }
                if (testMethod.TestClass.Class.Name != declaringType.FullName)
                {
                    return null;
                }
                return Exception?.InnerException;
            }
        }

        public StringBuilder? Builder;

        public void ThrowIfFlushed()
        {
            if (flushed)
            {
                throw new Exception("Logging context has been flushed.");
            }
        }

        internal bool flushed;

        internal Context(ITestOutputHelper testOutput, string sourceFilePath)
        {
            TestOutput = testOutput;
            SourceFilePath = sourceFilePath;
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
                Builder?.Append(value);
            }
        }

        public void Write(char value)
        {
            lock (locker)
            {
                ThrowIfFlushed();
                InitBuilder();
                Builder?.Append(value);
            }
        }

        public void WriteLine()
        {
            lock (locker)
            {
                ThrowIfFlushed();

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