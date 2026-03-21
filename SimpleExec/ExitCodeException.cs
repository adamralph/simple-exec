namespace SimpleExec;

/// <summary>
/// The command exited with an unexpected exit code.
/// </summary>
#pragma warning disable CA1032 // Implement standard exception constructors
public class ExitCodeException : Exception
#pragma warning restore CA1032
{
    internal ExitCodeException(int exitCode)
    {
        ExitCode = exitCode;
        Message = $"The command exited with code {ExitCode}.";
    }

    /// <summary>
    /// Gets the exit code of the command.
    /// </summary>
    public int ExitCode { get; }

    /// <inheritdoc/>
    public override string Message { get; }
}
