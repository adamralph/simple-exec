namespace SimpleExec
{
    /// <summary>
    /// The result of reading a command.
    /// </summary>
    public class ReadResult : CommandResult
    {
        /// <summary>
        /// Constructs an instance of a <see cref="ReadResult"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        /// <param name="out">The contents of standard output (stdout).</param>
        /// <param name="error">The contents of standard error (stderr).</param>
        public ReadResult(int exitCode, string @out, string error) : base(exitCode) => (this.Out, this.Error) = (@out, error);

        /// <summary>
        /// Gets the contents of standard output (stdout).
        /// </summary>
        public string Out { get; }

        /// <summary>
        /// Gets the contents of standard error (stderr).
        /// </summary>
        public string Error { get; }
    }
}
