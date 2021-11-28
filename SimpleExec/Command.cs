using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleExec
{
    /// <summary>
    /// Contains methods for running commands and reading standard output (stdout).
    /// </summary>
    public static class Command
    {
        private static readonly Action<IDictionary<string, string>> defaultAction = _ => { };
        private static readonly string defaultEchoPrefix = Assembly.GetEntryAssembly()?.GetName().Name ?? "SimpleExec";

        /// <summary>
        /// Runs a command without redirecting standard output (stdout) and standard error (stderr) and without writing to standard input (stdin).
        /// By default, the command line is echoed to standard error (stderr).
        /// </summary>
        /// <param name="name">The name of the command. This can be a path to an executable file.</param>
        /// <param name="args">The arguments to pass to the command.</param>
        /// <param name="workingDirectory">The working directory in which to run the command.</param>
        /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="windowsName">The name of the command to use on Windows only.</param>
        /// <param name="windowsArgs">The arguments to pass to the command on Windows only.</param>
        /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="handleExitCode">
        /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
        /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
        /// returns <see langword="false"/> otherwise.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static void Run(
            string name,
            string args = "",
            string workingDirectory = "",
            bool noEcho = false,
            string? windowsName = null,
            string? windowsArgs = null,
            string? echoPrefix = null,
            Action<IDictionary<string, string>>? configureEnvironment = null,
            bool createNoWindow = false,
            Func<int, bool>? handleExitCode = null,
            CancellationToken cancellationToken = default)
        {
            Validate(name);

            using var process = new Process();

            process.StartInfo = ProcessStartInfo.Create(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsName ?? name : name,
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsArgs ?? args : args,
                workingDirectory,
                false,
                configureEnvironment ?? defaultAction,
                createNoWindow);

            process.Run(noEcho, echoPrefix ?? defaultEchoPrefix, cancellationToken);

            if (!(handleExitCode?.Invoke(process.ExitCode) ?? false) && process.ExitCode != 0)
            {
                throw new ExitCodeException(process.ExitCode);
            }
        }

        /// <summary>
        /// Runs a command asynchronously without redirecting standard output (stdout) and standard error (stderr) and without writing to standard input (stdin).
        /// By default, the command line is echoed to standard error (stderr).
        /// </summary>
        /// <param name="name">The name of the command. This can be a path to an executable file.</param>
        /// <param name="args">The arguments to pass to the command.</param>
        /// <param name="workingDirectory">The working directory in which to run the command.</param>
        /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="windowsName">The name of the command to use on Windows only.</param>
        /// <param name="windowsArgs">The arguments to pass to the command on Windows only.</param>
        /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="handleExitCode">
        /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
        /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
        /// returns <see langword="false"/> otherwise.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
        /// <exception cref="ExitCodeReadException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task RunAsync(
            string name,
            string args = "",
            string workingDirectory = "",
            bool noEcho = false,
            string? windowsName = null,
            string? windowsArgs = null,
            string? echoPrefix = null,
            Action<IDictionary<string, string>>? configureEnvironment = null,
            bool createNoWindow = false,
            Func<int, bool>? handleExitCode = null,
            CancellationToken cancellationToken = default)
        {
            Validate(name);

            using var process = new Process();

            process.StartInfo = ProcessStartInfo.Create(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsName ?? name : name,
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsArgs ?? args : args,
                workingDirectory,
                false,
                configureEnvironment ?? defaultAction,
                createNoWindow);

            await process.RunAsync(noEcho, echoPrefix ?? defaultEchoPrefix, cancellationToken).ConfigureAwait(false);

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
        /// <param name="windowsName">The name of the command to use on Windows only.</param>
        /// <param name="windowsArgs">The arguments to pass to the command on Windows only.</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="encoding">The preferred <see cref="Encoding"/> for standard output (stdout) and standard error (stderr).</param>
        /// <param name="handleExitCode">
        /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
        /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
        /// returns <see langword="false"/> otherwise.
        /// </param>
        /// <param name="standardInput">The contents of standard input (stdin).</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous running of the command and reading of standard output (stdout) and standard error (stderr).
        /// The task result is a <see cref="Result"/> representing the contents of standard output (stdout) and standard error (stderr).
        /// </returns>
        /// <exception cref="ExitCodeReadException">
        /// The command exited with non-zero exit code. The exception contains the contents of standard output (stdout) and standard error (stderr).
        /// </exception>
        public static async Task<Result> ReadAsync(
            string name,
            string args = "",
            string workingDirectory = "",
            string? windowsName = null,
            string? windowsArgs = null,
            Action<IDictionary<string, string>>? configureEnvironment = null,
            Encoding? encoding = null,
            Func<int, bool>? handleExitCode = null,
            string? standardInput = null,
            CancellationToken cancellationToken = default)
        {
            Validate(name);

            using var process = new Process();

            process.StartInfo = ProcessStartInfo.Create(
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsName ?? name : name,
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsArgs ?? args : args,
                workingDirectory,
                true,
                configureEnvironment ?? defaultAction,
                true,
                encoding);

            var runProcess = process.RunAsync(true, defaultEchoPrefix, cancellationToken);

            Task<string> readOutput;
            Task<string> readError;

            try
            {
                await process.StandardInput.WriteAsync(standardInput).ConfigureAwait(false);
                process.StandardInput.Close();

                readOutput = process.StandardOutput.ReadToEndAsync();
                readError = process.StandardError.ReadToEndAsync();
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
                ? new Result(output, error)
                : throw new ExitCodeReadException(process.ExitCode, output, error);
        }

        private static void Validate(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The command name is missing.", nameof(name));
            }
        }
    }
}
