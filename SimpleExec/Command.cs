using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleExec;

/// <summary>
/// Contains methods for running commands and reading standard output (stdout).
/// </summary>
public static class Command
{
    private static readonly Action<IDictionary<string, string?>> defaultAction = _ => { };
    private static readonly string defaultEchoPrefix = Assembly.GetEntryAssembly()?.GetName().Name ?? "SimpleExec";

    /// <summary>
    /// Runs a command without redirecting standard output (stdout) and standard error (stderr) and without writing to standard input (stdin).
    /// By default, the command line is echoed to standard output (stdout).
    /// </summary>
    /// <param name="name">The name of the command. This can be a path to an executable file.</param>
    /// <param name="args">The arguments to pass to the command.</param>
    /// <param name="workingDirectory">The working directory in which to run the command.</param>
    /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
    /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
    /// <remarks>
    /// By default, the resulting command line and the working directory (if specified) are echoed to standard output (stdout).
    /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
    /// </remarks>
    public static void Run(
        string name,
        string args = "",
        string workingDirectory = "",
        bool noEcho = false,
        string? echoPrefix = null,
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        bool createNoWindow = false,
        Func<int, bool>? handleExitCode = null,
        bool cancellationIgnoresProcessTree = false,
        CancellationToken cancellationToken = default) =>
        ProcessStartInfo
            .Create(
                Resolve(Validate(name)),
                args,
#if NET8_0_OR_GREATER
                [],
#else
                Enumerable.Empty<string>(),
#endif
                workingDirectory,
                false,
                configureEnvironment ?? defaultAction,
                createNoWindow)
            .Run(noEcho, echoPrefix ?? defaultEchoPrefix, handleExitCode, cancellationIgnoresProcessTree, cancellationToken);

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
    /// <param name="noEcho">Whether or not to echo the resulting command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="echoPrefix">The prefix to use when echoing the command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
    /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
    public static void Run(
        string name,
        IEnumerable<string> args,
        string workingDirectory = "",
        bool noEcho = false,
        string? echoPrefix = null,
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        bool createNoWindow = false,
        Func<int, bool>? handleExitCode = null,
        bool cancellationIgnoresProcessTree = false,
        CancellationToken cancellationToken = default) =>
        ProcessStartInfo
            .Create(
                Resolve(Validate(name)),
                "",
                args ?? throw new ArgumentNullException(nameof(args)),
                workingDirectory,
                false,
                configureEnvironment ?? defaultAction,
                createNoWindow)
            .Run(noEcho, echoPrefix ?? defaultEchoPrefix, handleExitCode, cancellationIgnoresProcessTree, cancellationToken);

    private static void Run(
        this System.Diagnostics.ProcessStartInfo startInfo,
        bool noEcho,
        string echoPrefix,
        Func<int, bool>? handleExitCode,
        bool cancellationIgnoresProcessTree,
        CancellationToken cancellationToken)
    {
        using var process = new Process();
        process.StartInfo = startInfo;

        process.Run(noEcho, echoPrefix, cancellationIgnoresProcessTree, cancellationToken);

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
    /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard output (stdout).</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
    /// <exception cref="ExitCodeReadException">The command exited with non-zero exit code.</exception>
    /// <remarks>
    /// By default, the resulting command line and the working directory (if specified) are echoed to standard output (stdout).
    /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
    /// </remarks>
    public static Task RunAsync(
        string name,
        string args = "",
        string workingDirectory = "",
        bool noEcho = false,
        string? echoPrefix = null,
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        bool createNoWindow = false,
        Func<int, bool>? handleExitCode = null,
        bool cancellationIgnoresProcessTree = false,
        CancellationToken cancellationToken = default) =>
        ProcessStartInfo
            .Create(
                Resolve(Validate(name)),
                args,
#if NET8_0_OR_GREATER
                [],
#else
                Enumerable.Empty<string>(),
#endif
                workingDirectory,
                false,
                configureEnvironment ?? defaultAction,
                createNoWindow)
            .RunAsync(noEcho, echoPrefix ?? defaultEchoPrefix, handleExitCode, cancellationIgnoresProcessTree, cancellationToken);

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
    /// <param name="noEcho">Whether or not to echo the resulting command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="echoPrefix">The prefix to use when echoing the command name, arguments, and working directory (if specified) to standard output (stdout).</param>
    /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
    /// <param name="createNoWindow">Whether to run the command in a new window.</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
    /// <exception cref="ExitCodeReadException">The command exited with non-zero exit code.</exception>
    public static Task RunAsync(
        string name,
        IEnumerable<string> args,
        string workingDirectory = "",
        bool noEcho = false,
        string? echoPrefix = null,
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        bool createNoWindow = false,
        Func<int, bool>? handleExitCode = null,
        bool cancellationIgnoresProcessTree = false,
        CancellationToken cancellationToken = default) =>
        ProcessStartInfo
            .Create(
                Resolve(Validate(name)),
                "",
                args ?? throw new ArgumentNullException(nameof(args)),
                workingDirectory,
                false,
                configureEnvironment ?? defaultAction,
                createNoWindow)
            .RunAsync(noEcho, echoPrefix ?? defaultEchoPrefix, handleExitCode, cancellationIgnoresProcessTree, cancellationToken);

    private static async Task RunAsync(
        this System.Diagnostics.ProcessStartInfo startInfo,
        bool noEcho,
        string echoPrefix,
        Func<int, bool>? handleExitCode,
        bool cancellationIgnoresProcessTree,
        CancellationToken cancellationToken)
    {
        using var process = new Process();
        process.StartInfo = startInfo;

        await process.RunAsync(noEcho, echoPrefix, cancellationIgnoresProcessTree, cancellationToken).ConfigureAwait(false);

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
    /// <param name="encoding">The preferred <see cref="Encoding"/> for standard output (stdout) and standard output (stdout).</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="standardInput">The contents of standard input (stdin).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous running of the command and reading of standard output (stdout) and standard error (stderr).
    /// The task result is a <see cref="ValueTuple{T1, T2}"/> representing the contents of standard output (stdout) and standard error (stderr).
    /// </returns>
    /// <exception cref="ExitCodeReadException">
    /// The command exited with non-zero exit code. The exception contains the contents of standard output (stdout) and standard error (stderr).
    /// </exception>
    public static Task<(string StandardOutput, string StandardError)> ReadAsync(
        string name,
        string args = "",
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        Encoding? encoding = null,
        Func<int, bool>? handleExitCode = null,
        string? standardInput = null,
        bool cancellationIgnoresProcessTree = false,
        CancellationToken cancellationToken = default) =>
        ProcessStartInfo
            .Create(
                Resolve(Validate(name)),
                args,
#if NET8_0_OR_GREATER
                [],
#else
                Enumerable.Empty<string>(),
#endif
                workingDirectory,
                true,
                configureEnvironment ?? defaultAction,
                true,
                encoding)
            .ReadAsync(
                handleExitCode,
                standardInput,
                cancellationIgnoresProcessTree,
                cancellationToken);

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
    /// <param name="encoding">The preferred <see cref="Encoding"/> for standard output (stdout) and standard error (stderr).</param>
    /// <param name="handleExitCode">
    /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
    /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
    /// returns <see langword="false"/> otherwise.
    /// </param>
    /// <param name="standardInput">The contents of standard input (stdin).</param>
    /// <param name="cancellationIgnoresProcessTree">
    /// Whether to ignore the process tree when cancelling the command.
    /// If set to <c>true</c>, when the command is cancelled, any child processes created by the command
    /// are left running after the command is cancelled.
    /// </param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous running of the command and reading of standard output (stdout) and standard error (stderr).
    /// The task result is a <see cref="ValueTuple{T1, T2}"/> representing the contents of standard output (stdout) and standard error (stderr).
    /// </returns>
    /// <exception cref="ExitCodeReadException">
    /// The command exited with non-zero exit code. The exception contains the contents of standard output (stdout) and standard error (stderr).
    /// </exception>
    public static Task<(string StandardOutput, string StandardError)> ReadAsync(
        string name,
        IEnumerable<string> args,
        string workingDirectory = "",
        Action<IDictionary<string, string?>>? configureEnvironment = null,
        Encoding? encoding = null,
        Func<int, bool>? handleExitCode = null,
        string? standardInput = null,
        bool cancellationIgnoresProcessTree = false,
        CancellationToken cancellationToken = default) =>
        ProcessStartInfo
            .Create(
                Resolve(Validate(name)),
                "",
                args ?? throw new ArgumentNullException(nameof(args)),
                workingDirectory,
                true,
                configureEnvironment ?? defaultAction,
                true,
                encoding)
            .ReadAsync(
                handleExitCode,
                standardInput,
                cancellationIgnoresProcessTree,
                cancellationToken);

    private static async Task<(string StandardOutput, string StandardError)> ReadAsync(
        this System.Diagnostics.ProcessStartInfo startInfo,
        Func<int, bool>? handleExitCode,
        string? standardInput,
        bool cancellationIgnoresProcessTree,
        CancellationToken cancellationToken)
    {
        using var process = new Process();
        process.StartInfo = startInfo;

        var runProcess = process.RunAsync(true, "", cancellationIgnoresProcessTree, cancellationToken);

        Task<string> readOutput;
        Task<string> readError;

        try
        {
            await process.StandardInput.WriteAsync(standardInput).ConfigureAwait(false);
            process.StandardInput.Close();

#if NET8_0_OR_GREATER
            readOutput = process.StandardOutput.ReadToEndAsync(cancellationToken);
            readError = process.StandardError.ReadToEndAsync(cancellationToken);
#else
            readOutput = process.StandardOutput.ReadToEndAsync();
            readError = process.StandardError.ReadToEndAsync();
#endif
        }
        catch (Exception)
        {
            await runProcess.ConfigureAwait(false);
            throw;
        }

        await Task.WhenAll(runProcess, readOutput, readError).ConfigureAwait(false);

#pragma warning disable CA1849 // Call async methods when in an async method
        var output = readOutput.Result;
        var error = readError.Result;
#pragma warning restore CA1849 // Call async methods when in an async method

        return (handleExitCode?.Invoke(process.ExitCode) ?? false) || process.ExitCode == 0
            ? (output, error)
            : throw new ExitCodeReadException(process.ExitCode, output, error);
    }

    private static string Validate(string name) =>
        string.IsNullOrWhiteSpace(name)
            ? throw new ArgumentException("The command name is missing.", nameof(name))
            : name;

    private static string Resolve(string name)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || Path.IsPathRooted(name))
        {
            return name;
        }

        var extension = Path.GetExtension(name);
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
            ? windowsExecutableExtensions.Select(ex => Path.ChangeExtension(name, ex)).ToList()
#if NET8_0_OR_GREATER
            : [name];
#else
            : new List<string> { name, };
#endif

        var path = GetSearchDirectories().SelectMany(_ => searchFileNames, Path.Combine)
            .FirstOrDefault(File.Exists);

        return path == null || Path.GetExtension(path) == ".exe" ? name : path;
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
