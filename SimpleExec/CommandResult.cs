namespace SimpleExec
{
    /// <summary>
    /// The result of running a command.
    /// </summary>
    public abstract class CommandResult
    {
        /// <summary>
        /// Constructs an instance of a <see cref="CommandResult"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        protected CommandResult(int exitCode) => this.ExitCode = exitCode;

        /// <summary>
        /// Gets the exit code of the command.
        /// </summary>
        public int ExitCode { get; }
    }
}
