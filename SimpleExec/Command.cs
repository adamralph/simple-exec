using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleExec;

/// <summary>
/// Contains methods for running commands and reading standard output (stdout).
/// </summary>
#pragma warning disable RS0026 // Do not add multiple overloads with optional parameters
public static class Command
{
    private static readonly Action<IDictionary<string, string?>> DefaultAction = _ => { };
    private static readonly string DefaultEchoPrefix = Assembly.GetEntryAssembly()?.GetName().Name ?? "SimpleExec";

    /// <summary>
    /// Runs a command without redirecting standard output (stdout) and standard error (stderr) and without writing to standard input (stdin).
    /// By default, the command line is echoed to standard output (stdout).
    /// </summary>
    /// <param name="name">The name of the command. This can be a path to an executable file.</param>
    /// <param name="args">The arguments to pass to the command.</param>
    /// <param name="workingDirectory">The working directory in which to run the command.</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="secrets">A list of secrets that are redacted by replacement with "***" when echoing the resulting command line and the working directory (if specified) to standard output (stdout).</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="noEcho">Whether to echo the resulting command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="ct">A <see cref="Ct"/> to observe while waiting for the command to exit.</param>
    /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
    /// <remarks>
    /// By default, the resulting command line and the working directory (if specified) are echoed to standard output (stdout).
    /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
    /// </remarks>
    public static void Run(
        CommandName name,
        string args = "",
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        IEnumerable<string>? secrets = null,
        Func<int, bool>? handleExitCode = null,
        string? echoPrefix = null,
        bool noEcho = false,
        bool cancellationIgnoresProcessTree = false,
        bool createNoWindow = false,
        Ct ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        ProcessStartInfo
            .Create(
                Resolve(name),
                args,
                [],
                workingDirectory,
                configureEnvironment ?? DefaultAction,
                null,
                createNoWindow,
                false)
            .Run(
                secrets ?? [],
                handleExitCode,
                echoPrefix ?? DefaultEchoPrefix,
                noEcho,
                cancellationIgnoresProcessTree,
                ct);
    }

    /// <summary>
    /// Runs a command without redirecting standard output (stdout) and standard error (stderr) and without writing to standard input (stdin).
    /// By default, the command line is echoed to standard output (stdout).
    /// </summary>
    /// <param name="name">The name of the command. This can be a path to an executable file.</param>
    /// <param name="args">
    /// The arguments to pass to the command.
    /// As with <see cref="System.Diagnostics.ProcessStartInfo.ArgumentList"/>, the strings don't need to be escaped.
    /// </param>
    /// <param name="workingDirectory">The working directory in which to run the command.</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="secrets">A list of secrets that are redacted by replacement with "***" when echoing the resulting command line and the working directory (if specified) to standard output (stdout).</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="echoPrefix">The prefix to use when echoing the command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="noEcho">Whether to echo the resulting command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="ct">A <see cref="Ct"/> to observe while waiting for the command to exit.</param>
    /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
    public static void Run(
        CommandName name,
        IEnumerable<string> args,
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        IEnumerable<string>? secrets = null,
        Func<int, bool>? handleExitCode = null,
        string? echoPrefix = null,
        bool noEcho = false,
        bool cancellationIgnoresProcessTree = false,
        bool createNoWindow = false,
        Ct ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        ProcessStartInfo
            .Create(
                Resolve(name),
                "",
                args ?? throw new ArgumentNullException(nameof(args)),
                workingDirectory,
                configureEnvironment ?? DefaultAction,
                null,
                createNoWindow,
                false)
            .Run(
                secrets ?? [],
                handleExitCode,
                echoPrefix ?? DefaultEchoPrefix,
                noEcho,
                cancellationIgnoresProcessTree,
                ct);
    }

    private static void Run(
        this System.Diagnostics.ProcessStartInfo startInfo,
        IEnumerable<string> secrets,
        Func<int, bool>? handleExitCode,
        string echoPrefix,
        bool noEcho,
        bool cancellationIgnoresProcessTree,
        Ct ct)
    {
        using var process = new Process();
        process.StartInfo = startInfo;

        process.Run(secrets, echoPrefix, noEcho, cancellationIgnoresProcessTree, ct);

        if (!(handleExitCode?.Invoke(process.ExitCode) ?? false) && process.ExitCode != 0)
        {
            throw new ExitCodeException(process.ExitCode);
        }
    }

    /// <summary>
    /// Runs a command asynchronously without redirecting standard output (stdout) and standard error (stderr) and without writing to standard input (stdin).
    /// By default, the command line is echoed to standard output (stdout).
    /// </summary>
    /// <param name="name">The name of the command. This can be a path to an executable file.</param>
    /// <param name="args">The arguments to pass to the command.</param>
    /// <param name="workingDirectory">The working directory in which to run the command.</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="secrets">A list of secrets that are redacted by replacement with "***" when echoing the resulting command line and the working directory (if specified) to standard output (stdout).</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="noEcho">Whether to echo the resulting command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="ct">A <see cref="Ct"/> to observe while waiting for the command to exit.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
    /// <exception cref="ExitCodeReadException">The command exited with non-zero exit code.</exception>
    /// <remarks>
    /// By default, the resulting command line and the working directory (if specified) are echoed to standard output (stdout).
    /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
    /// </remarks>
    public static Task RunAsync(
        CommandName name,
        string args = "",
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        IEnumerable<string>? secrets = null,
        Func<int, bool>? handleExitCode = null,
        string? echoPrefix = null,
        bool noEcho = false,
        bool cancellationIgnoresProcessTree = false,
        bool createNoWindow = false,
        Ct ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        return ProcessStartInfo
            .Create(
                Resolve(name),
                args,
                [],
                workingDirectory,
                configureEnvironment ?? DefaultAction,
                null,
                createNoWindow,
                false)
            .RunAsync(
                secrets ?? [],
                handleExitCode,
                echoPrefix ?? DefaultEchoPrefix,
                noEcho,
                cancellationIgnoresProcessTree,
                ct);
    }

    /// <summary>
    /// Runs a command asynchronously without redirecting standard output (stdout) and standard error (stderr) and without writing to standard input (stdin).
    /// By default, the command line is echoed to standard output (stdout).
    /// </summary>
    /// <param name="name">The name of the command. This can be a path to an executable file.</param>
    /// <param name="args">
    /// The arguments to pass to the command.
    /// As with <see cref="System.Diagnostics.ProcessStartInfo.ArgumentList"/>, the strings don't need to be escaped.
    /// </param>
    /// <param name="workingDirectory">The working directory in which to run the command.</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="secrets">A list of secrets that are redacted by replacement with "***" when echoing the resulting command line and the working directory (if specified) to standard output (stdout).</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="echoPrefix">The prefix to use when echoing the command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="noEcho">Whether to echo the resulting command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="ct">A <see cref="Ct"/> to observe while waiting for the command to exit.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
    /// <exception cref="ExitCodeReadException">The command exited with non-zero exit code.</exception>
    public static Task RunAsync(
        CommandName name,
        IEnumerable<string> args,
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        IEnumerable<string>? secrets = null,
        Func<int, bool>? handleExitCode = null,
        string? echoPrefix = null,
        bool noEcho = false,
        bool cancellationIgnoresProcessTree = false,
        bool createNoWindow = false,
        Ct ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        return ProcessStartInfo
            .Create(
                Resolve(name),
                "",
                args ?? throw new ArgumentNullException(nameof(args)),
                workingDirectory,
                configureEnvironment ?? DefaultAction,
                null,
                createNoWindow,
                false)
            .RunAsync(
                secrets ?? [],
                handleExitCode,
                echoPrefix ?? DefaultEchoPrefix,
                noEcho,
                cancellationIgnoresProcessTree,
                ct);
    }

    private static async Task RunAsync(
        this System.Diagnostics.ProcessStartInfo startInfo,
        IEnumerable<string> secrets,
        Func<int, bool>? handleExitCode,
        string echoPrefix,
        bool noEcho,
        bool cancellationIgnoresProcessTree,
        Ct ct)
    {
        using var process = new Process();
        process.StartInfo = startInfo;

        await process.RunAsync(secrets, echoPrefix, noEcho, cancellationIgnoresProcessTree, ct).ConfigureAwait(false);

        if (!(handleExitCode?.Invoke(process.ExitCode) ?? false) && process.ExitCode != 0)
        {
            throw new ExitCodeException(process.ExitCode);
        }
    }

    /// <summary>
    /// Runs a command and reads standard output (stdout) and standard error (stderr) and optionally writes to standard input (stdin).
    /// </summary>
    /// <param name="name">The name of the command. This can be a path to an executable file.</param>
    /// <param name="args">The arguments to pass to the command.</param>
    /// <param name="workingDirectory">The working directory in which to run the command.</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="encoding">The preferred <see cref="Encoding"/> for standard output (stdout) and standard output (stdout).</param>
    /// <param name="standardInput">The contents of standard input (stdin).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="ct">A <see cref="Ct"/> to observe while waiting for the command to exit.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous running of the command and reading of standard output (stdout) and standard error (stderr).
    /// The task result is a <see cref="ValueTuple{T1, T2}"/> representing the contents of standard output (stdout) and standard error (stderr).
    /// </returns>
    /// <exception cref="ExitCodeReadException">
    /// The command exited with non-zero exit code. The exception contains the contents of standard output (stdout) and standard error (stderr).
    /// </exception>
    public static Task<(string StandardOutput, string StandardError)> ReadAsync(
        CommandName name,
        string args = "",
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        Func<int, bool>? handleExitCode = null,
        Encoding? encoding = null,
        string? standardInput = null,
        bool cancellationIgnoresProcessTree = false,
        Ct ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        return ProcessStartInfo
            .Create(
                Resolve(name),
                args,
                [],
                workingDirectory,
                configureEnvironment ?? DefaultAction,
                encoding,
                true,
                true)
            .ReadAsync(
                handleExitCode,
                standardInput,
                cancellationIgnoresProcessTree,
                ct);
    }

    /// <summary>
    /// Runs a command and reads standard output (stdout) and standard error (stderr) and optionally writes to standard input (stdin).
    /// </summary>
    /// <param name="name">The name of the command. This can be a path to an executable file.</param>
    /// <param name="args">
    /// The arguments to pass to the command.
    /// As with <see cref="System.Diagnostics.ProcessStartInfo.ArgumentList"/>, the strings don't need to be escaped.
    /// </param>
    /// <param name="workingDirectory">The working directory in which to run the command.</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="encoding">The preferred <see cref="Encoding"/> for standard output (stdout) and standard error (stderr).</param>
    /// <param name="standardInput">The contents of standard input (stdin).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="ct">A <see cref="Ct"/> to observe while waiting for the command to exit.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous running of the command and reading of standard output (stdout) and standard error (stderr).
    /// The task result is a <see cref="ValueTuple{T1, T2}"/> representing the contents of standard output (stdout) and standard error (stderr).
    /// </returns>
    /// <exception cref="ExitCodeReadException">
    /// The command exited with non-zero exit code. The exception contains the contents of standard output (stdout) and standard error (stderr).
    /// </exception>
    public static Task<(string StandardOutput, string StandardError)> ReadAsync(
        CommandName name,
        IEnumerable<string> args,
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        Func<int, bool>? handleExitCode = null,
        Encoding? encoding = null,
        string? standardInput = null,
        bool cancellationIgnoresProcessTree = false,
        Ct ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);

        return ProcessStartInfo
            .Create(
                Resolve(name),
                "",
                args ?? throw new ArgumentNullException(nameof(args)),
                workingDirectory,
                configureEnvironment ?? DefaultAction,
                encoding,
                true,
                true)
            .ReadAsync(
                handleExitCode,
                standardInput,
                cancellationIgnoresProcessTree,
                ct);
    }

    private static async Task<(string StandardOutput, string StandardError)> ReadAsync(
        this System.Diagnostics.ProcessStartInfo startInfo,
        Func<int, bool>? handleExitCode,
        string? standardInput,
        bool cancellationIgnoresProcessTree,
        Ct ct)
    {
        using var process = new Process();
        process.StartInfo = startInfo;

#pragma warning disable CA2025 // Do not pass 'IDisposable' instances into unawaited tasks
        var runProcess = process.RunAsync([], "", true, cancellationIgnoresProcessTree, ct);
#pragma warning restore CA2025

        Task<string> readOutput;
        Task<string> readError;

        try
        {
            await process.StandardInput.WriteAsync(standardInput).ConfigureAwait(false);
            process.StandardInput.Close();

            readOutput = process.StandardOutput.ReadToEndAsync(ct);
            readError = process.StandardError.ReadToEndAsync(ct);
        }
        catch (Exception)
        {
            await runProcess.ConfigureAwait(false);
            throw;
        }

        await Task.WhenAll(runProcess, readOutput, readError).ConfigureAwait(false);

#pragma warning disable CA1849 // Call async methods when in an async method
#pragma warning disable VSTHRD103 // Result synchronously blocks. Use await instead.
        var output = readOutput.Result;
        var error = readError.Result;
#pragma warning restore VSTHRD103
#pragma warning restore CA1849

        return (handleExitCode?.Invoke(process.ExitCode) ?? false) || process.ExitCode == 0
            ? (output, error)
            : throw new ExitCodeReadException(process.ExitCode, output, error);
    }

    private static CommandName Resolve(CommandName name)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || Path.IsPathRooted(name.Name))
        {
            return name;
        }

        var extension = Path.GetExtension(name.Name);
        if (!string.IsNullOrEmpty(extension) && extension != ".cmd" && extension != ".bat")
        {
            return name;
        }

        var pathExt = Environment.GetEnvironmentVariable("PATHEXT") ?? ".EXE;.BAT;.CMD";

        var windowsExecutableExtensions = pathExt.Split(';')
            .Select(ext => ext.TrimStart('.'))
            .Where(ext =>
                string.Equals(ext, "exe", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, "bat", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(ext, "cmd", StringComparison.OrdinalIgnoreCase));

        var searchFileNames = string.IsNullOrEmpty(extension)
            ? windowsExecutableExtensions.Select(ex => Path.ChangeExtension(name.Name, ex)).ToList()
            : [name.Name,];

        var path = GetSearchDirectories().SelectMany(_ => searchFileNames, Path.Combine)
            .FirstOrDefault(File.Exists);

        return path == null || Path.GetExtension(path) == ".exe" ? name : new CommandName(path);
    }

    // see https://github.com/dotnet/runtime/blob/14304eb31eea134db58870a6d87312231b1e02b6/src/libraries/System.Diagnostics.Process/src/System/Diagnostics/Process.Unix.cs#L703-L726
    private static IEnumerable<string> GetSearchDirectories()
    {
        var currentProcessPath = Process.GetCurrentProcess().MainModule?.FileName;
        if (!string.IsNullOrEmpty(currentProcessPath))
        {
            var currentProcessDirectory = Path.GetDirectoryName(currentProcessPath);
            if (!string.IsNullOrEmpty(currentProcessDirectory))
            {
                yield return currentProcessDirectory;
            }
        }

        yield return Directory.GetCurrentDirectory();

        var path = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrEmpty(path))
        {
            yield break;
        }

        foreach (var directory in path.Split(Path.PathSeparator))
        {
            yield return directory;
        }
    }
}

/// <summary>
/// The name of a command.
/// </summary>
public record CommandName
{
    /// <summary>
    /// Constructs an instance of <see cref="CommandName"/>.
    /// </summary>
    /// <param name="name">The name of the command</param>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException"><paramref name="name"/> is empty or whitespace.</exception>
    public CommandName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be empty or whitespace.", nameof(name));
        }

        Name = name;
    }

    /// <summary>
    /// The name of the command.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Implicitly converts <paramref name="name"/> to an instance of <see cref="CommandName"/>.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <returns>An instance of <see cref="CommandName"/>.</returns>
    public static implicit operator CommandName(string name) => new(name);

    /// <summary>
    /// Creates an instance of <see cref="CommandName"/> from <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The name of the command.</param>
    /// <returns>An instance of <see cref="CommandName"/>.</returns>
    public static CommandName FromString(string name) => name;

    /// <inheritdoc/>
    public override string ToString() => Name;
}
