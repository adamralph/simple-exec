namespace SimpleExec
{
    using System;

    /// <summary>
    /// The command exited with a non-zero exit code.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
    public class NonZeroExitCodeException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
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
