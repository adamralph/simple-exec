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
    public ExitCodeReadException(int exitCode, string standardOutput, string standardError) :
        base(exitCode, CreateMessage(exitCode, standardOutput, standardError))
    {
        StandardOutput = standardOutput;
        StandardError = standardError;
    }

    /// <summary>
    /// Constructs an instance of a <see cref="ExitCodeReadException"/>.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="standardOutput">The contents of standard output (stdout).</param>
    /// <param name="standardError">The contents of standard error (stderr).</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception,
    /// or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.
    /// </param>
    public ExitCodeReadException(int exitCode, string standardOutput, string standardError, Exception innerException) :
        base(exitCode, CreateMessage(exitCode, standardOutput, standardError), innerException)
    {
        StandardOutput = standardOutput;
        StandardError = standardError;
    }

    /// <summary>
    /// Constructs an instance of a <see cref="ExitCodeReadException"/>.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="standardOutput">The contents of standard output (stdout).</param>
    /// <param name="standardError">The contents of standard error (stderr).</param>
    /// <param name="message">The message that describes the error.</param>
    public ExitCodeReadException(int exitCode, string standardOutput, string standardError, string message) :
        base(exitCode, message)
    {
        StandardOutput = standardOutput;
        StandardError = standardError;
    }

    /// <summary>
    /// Constructs an instance of a <see cref="ExitCodeReadException"/>.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="standardOutput">The contents of standard output (stdout).</param>
    /// <param name="standardError">The contents of standard error (stderr).</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception,
    /// or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.
    /// </param>
    public ExitCodeReadException(
        int exitCode, string standardOutput, string standardError, string message, Exception innerException) :
        base(exitCode, message, innerException)
    {
        StandardOutput = standardOutput;
        StandardError = standardError;
    }

    /// <summary>
    /// Gets the contents of standard output (stdout).
    /// </summary>
    public string StandardOutput { get; }

    /// <summary>
    /// Gets the contents of standard error (stderr).
    /// </summary>
    public string StandardError { get; }

    /// <summary>
    /// Create the message that describes the error.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="standardOutput">The contents of standard output (stdout).</param>
    /// <param name="standardError">The contents of standard error (stderr).</param>
    /// <returns>The message that describes the error</returns>
    protected static string CreateMessage(int exitCode, string standardOutput, string standardError) =>
        $"{CreateMessage(exitCode)}{TwoNewLines}Standard output (stdout):{TwoNewLines}{standardOutput}{TwoNewLines}Standard error (stderr):{TwoNewLines}{standardError}";
}
