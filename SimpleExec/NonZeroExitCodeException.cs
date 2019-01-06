namespace SimpleExec
{
    using System;

#pragma warning disable CA1032 // Implement standard exception constructors
    public class NonZeroExitCodeException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public NonZeroExitCodeException(int exitCode) : base($"The process exited with code {exitCode}.") => this.ExitCode = exitCode;

        public int ExitCode { get; }
    }
}
