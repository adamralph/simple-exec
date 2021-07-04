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
        /// <param name="error">The contents of standard error (stderror).</param>
        public ExitCodeException(int exitCode, string error) : base($"The command exited with code {exitCode}.") => (this.ExitCode, this.Error) = (exitCode, error);

        /// <summary>
        /// Gets the exit code of the command.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Gets the contents of standard error (stderror).
        /// </summary>
        public string Error { get; }
    }
}
