namespace SimpleExec
{
#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CS0618 // Type or member is obsolete
    public class NonZeroExitCodeException : CommandException
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        public NonZeroExitCodeException(int exitCode) : base($"The process exited with code {exitCode}.") => this.ExitCode = exitCode;

        public int ExitCode { get; }
    }
}
