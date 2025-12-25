namespace SimpleExec;

/// <summary>
/// The command being read exited with an unexpected exit code.
/// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
public class ExitCodeReadException : ExitCodeException
#pragma warning restore CA1032
{
    private static readonly string TwoNewLines = $"{Environment.NewLine}{Environment.NewLine}";

    /// <summary>
    /// Constructs an instance of a <see cref="ExitCodeReadException"/>.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="standardOutput">The contents of standard output (stdout).</param>
    /// <param name="standardError">The contents of standard error (stderr).</param>
    public ExitCodeReadException(int exitCode, string standardOutput, string standardError) : base(exitCode) => (StandardOutput, StandardError) = (standardOutput, standardError);

    /// <summary>
    /// Gets the contents of standard output (stdout).
    /// </summary>
    public string StandardOutput { get; }

    /// <summary>
    /// Gets the contents of standard error (stderr).
    /// </summary>
    public string StandardError { get; }

    /// <inheritdoc/>
    public override string Message =>
        $"{base.Message}{TwoNewLines}Standard output (stdout):{TwoNewLines}{StandardOutput}{TwoNewLines}Standard error (stderr):{TwoNewLines}{StandardError}";
}
