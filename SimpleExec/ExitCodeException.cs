namespace SimpleExec;

/// <summary>
/// The command exited with an unexpected exit code.
/// </summary>
/// <param name="exitCode">The exit code of the command.</param>
#pragma warning disable CA1032 // Implement standard exception constructors
public class ExitCodeException(int exitCode) : Exception
#pragma warning restore CA1032
{
    /// <summary>
    /// Gets the exit code of the command.
    /// </summary>
    public int ExitCode { get; } = exitCode;

    /// <inheritdoc/>
    public override string Message => $"The command exited with code {ExitCode}.";
}
