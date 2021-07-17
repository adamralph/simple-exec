namespace SimpleExec
{
    /// <summary>
    /// The command being run exited with an unexpected exit code.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
    public class RunException : ExitCodeException
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Constructs an instance of a <see cref="RunException"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        public RunException(int exitCode) : base(exitCode) { }
    }
}
