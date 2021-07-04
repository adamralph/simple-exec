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
        /// <param name="standardOutput">The contents of standard output (stdout).</param>
        /// <param name="standardError">The contents of standard error (stderr).</param>
        public Result(string standardOutput, string standardError) => (this.StandardOutput, this.StandardError) = (standardOutput, standardError);

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
