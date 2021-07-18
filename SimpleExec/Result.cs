namespace SimpleExec
{
    /// <summary>
    /// The result of reading a command.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Constructs an instance of a <see cref="Result"/>.
        /// </summary>
        /// <param name="out">The contents of standard output (stdout).</param>
        /// <param name="error">The contents of standard error (stderr).</param>
        public Result(string @out, string error) => (this.StandardOutput, this.StandardError) = (@out, error);

        /// <summary>
        /// Gets the contents of standard output (stdout).
        /// </summary>
        public string StandardOutput { get; }

        /// <summary>
        /// Gets the contents of standard error (stderr).
        /// </summary>
        public string StandardError { get; }
    }
}
