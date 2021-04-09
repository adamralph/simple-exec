using System;

namespace SimpleExec
{
    /// <summary>
    /// The command exited with a non-zero exit code.
    /// </summary>
    public class NonZeroExitCodeException : Exception
    {
        /// <summary>
        /// Constructs an instance of a <see cref="NonZeroExitCodeException"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        public NonZeroExitCodeException(int exitCode) : base($"The command exited with code {exitCode}.") => this.ExitCode = exitCode;

        /// <summary>
        /// Gets the exit code of the command.
        /// </summary>
        public int ExitCode { get; }
    }
}
