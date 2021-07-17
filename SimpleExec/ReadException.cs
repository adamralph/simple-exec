using System;

namespace SimpleExec
{
    /// <summary>
    /// The command being read exited with an unexpected exit code.
    /// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
    public class ReadException : ExitCodeException
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        private static readonly string twoNewLines = $"{Environment.NewLine}{Environment.NewLine}";

        /// <summary>
        /// Constructs an instance of a <see cref="ReadException"/>.
        /// </summary>
        /// <param name="exitCode">The exit code of the command.</param>
        /// <param name="out">The contents of standard output (stdour).</param>
        /// <param name="error">The contents of standard error (stderr).</param>
        public ReadException(int exitCode, string @out, string error) : base(exitCode) => (this.Out, this.Error) = (@out, error);

        /// <summary>
        /// Gets the contents of standard output (stdout).
        /// </summary>
        public string Out { get; }

        /// <summary>
        /// Gets the contents of standard error (stderr).
        /// </summary>
        public string Error { get; }

        /// <inheritdoc/>
        public override string Message =>
            $"{base.Message}{twoNewLines}Standard output:{twoNewLines}{this.Out}{twoNewLines}Standard error:{twoNewLines}{this.Error}";
    }
}
