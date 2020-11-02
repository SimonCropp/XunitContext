using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace Xunit
{
    public abstract class XunitContextBase :
        IDisposable
    {
        /// <summary>
        /// The current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public ITestOutputHelper Output { get; }

        public Context Context { get; }

        protected XunitContextBase(
            ITestOutputHelper output,
            [CallerFilePath] string sourceFile = "")
        {
            Guard.AgainstNull(output, nameof(output));
            Guard.AgainstNullOrEmpty(sourceFile, nameof(sourceFile));

            Output = output;
            Context = XunitContext.Register(output, sourceFile);
        }

        /// <summary>
        /// Writes a value to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void WriteLine(object value)
        {
            Context.WriteLine(value);
        }

        /// <summary>
        /// Writes a line to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void WriteLine(string value)
        {
            Context.WriteLine(value);
        }

        /// <summary>
        /// Writes a <see cref="char"/> to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void Write(char value)
        {
            Context.Write(value);
        }

        /// <summary>
        /// Writes a line to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void WriteLine()
        {
            Context.WriteLine();
        }

        /// <summary>
        /// Writes a value to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void Write(object value)
        {
            Context.Write(value);
        }

        /// <summary>
        /// Writes a string to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void Write(string value)
        {
            Context.Write(value);
        }

        /// <summary>
        /// All log message that have been written to the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public IReadOnlyList<string> Logs => Context.LogMessages;

        public virtual void Dispose()
        {
            Context.Flush();
        }

        /// <summary>
        /// The <see cref="Exception"/> for the current test if it failed.
        /// </summary>
        public Exception? TestException
        {
            get => Context.TestException;
        }

        /// <summary>
        /// The source file that the current test exists in.
        /// </summary>
        public string SourceFile
        {
            get => Context.SourceFile;
        }

        /// <summary>
        /// The source directory that the current test exists in.
        /// </summary>
        public string SourceDirectory
        {
            get => Context.SourceDirectory;
        }

        /// <summary>
        /// The current solution directory. Obtained by walking up the directory tree from <see cref="SourceDirectory"/>.
        /// </summary>
        public string SolutionDirectory
        {
            get => Context.SolutionDirectory;
        }

        /// <summary>
        /// Override the default parameter resolution.
        /// </summary>
        public void UseParameters(params object[] parameters)
        {
            Guard.AgainstNull(parameters, nameof(parameters));
            Context.UseParameters(parameters);
        }
    }
}