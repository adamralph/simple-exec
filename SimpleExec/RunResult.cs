namespace SimpleExec
{
    /// <summary>
    /// The result of running a command.
    /// </summary>
    public class RunResult : CommandResult
    {
        /// <summary>
        /// Constructs an instance of a <see cref="RunResult"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        public RunResult(int exitCode) : base(exitCode) { }
    }
}
