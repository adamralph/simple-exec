namespace SimpleExec
{
    /// <summary>
    /// The command exited with an unexpected exit code.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CS0618 // Type or member is obsolete
    public class ExitCodeException : NonZeroExitCodeException
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Constructs an instance of a <see cref="ExitCodeException"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        public ExitCodeException(int exitCode) : base(exitCode) { }
    }
}
