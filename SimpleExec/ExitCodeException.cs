using System;

namespace SimpleExec
{
    /// <summary>
    /// The command exited with an unexpected exit code.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
    public class ExitCodeException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Constructs an instance of a <see cref="ExitCodeException"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        public ExitCodeException(int exitCode) : base($"The command exited with code {exitCode}.") => this.ExitCode = exitCode;

        /// <summary>
        /// Gets the exit code of the command.
        /// </summary>
        public int ExitCode { get; }
    }
}
