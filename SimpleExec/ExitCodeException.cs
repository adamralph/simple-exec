namespace SimpleExec;

/// <summary>
/// The command exited with an unexpected exit code.
/// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
public class ExitCodeException : Exception
#pragma warning restore CA1032
{
    /// <summary>
    ///     Constructs an instance of a <see cref="ExitCodeException" />.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    public ExitCodeException(int exitCode) :
        base(CreateMessage(exitCode)) =>
        ExitCode = exitCode;

    /// <summary>
    ///     Constructs an instance of a <see cref="ExitCodeException" />.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception,
    ///     or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.
    /// </param>
    public ExitCodeException(int exitCode, Exception innerException) :
        base(CreateMessage(exitCode), innerException) =>
        ExitCode = exitCode;

    /// <summary>
    ///     Constructs an instance of a <see cref="ExitCodeException" />.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="message">The message that describes the error.</param>
    public ExitCodeException(int exitCode, string message) :
        base(message) =>
        ExitCode = exitCode;

    /// <summary>
    ///     Constructs an instance of a <see cref="ExitCodeException" />.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception,
    ///     or a <c>null</c> reference (<c>Nothing</c> in Visual Basic) if no inner exception is specified.
    /// </param>
    public ExitCodeException(int exitCode, string message, Exception innerException) :
        base(message, innerException) =>
        ExitCode = exitCode;

    /// <summary>
    /// Gets the exit code of the command.
    /// </summary>
    public int ExitCode { get; }

    /// <summary>
    /// Create the message that describes the error.
    /// </summary>
    /// <param name="exitCode">The exit code of the command.</param>
    /// <returns>The message that describes the error</returns>
    protected static string CreateMessage(int exitCode) => $"The command exited with code {exitCode}.";
}
