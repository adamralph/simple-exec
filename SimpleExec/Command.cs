using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <summary>
        /// Runs a command.
        /// By default, the command line is echoed to standard error (stderr).
        /// </summary>
        /// <param name="name">The name of the command. This can be a path to an executable file.</param>
        /// <param name="args">The arguments to pass to the command.</param>
        /// <param name="workingDirectory">The working directory in which to run the command.</param>
        /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="windowsName">The name of the command to use on Windows only.</param>
        /// <param name="windowsArgs">The arguments to pass to the command on Windows only.</param>
        /// <param name="logPrefix">The prefix to use when logging messages to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="handleExitCode">
        /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
        /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
        /// returns <see langword="false"/> otherwise.
        /// </param>
        /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static void Run(
            string name,
            string args = null,
            string workingDirectory = null,
            bool noEcho = false,
            string windowsName = null,
            string windowsArgs = null,
            string logPrefix = null,
            Action<IDictionary<string, string>> configureEnvironment = null,
            bool createNoWindow = false,
            Func<int, bool> handleExitCode = null)
        {
            Validate(name);

            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(
                    name,
                    args,
                    workingDirectory,
                    false,
                    windowsName,
                    windowsArgs,
                    configureEnvironment,
                    createNoWindow,
                    null);

                process.Run(noEcho, logPrefix ?? DefaultPrefix.Value);

                if (!(handleExitCode?.Invoke(process.ExitCode) ?? false) && process.ExitCode != 0)
                {
                    process.Throw();
                }
            }
        }

        /// <summary>
        /// Runs a command asynchronously.
        /// By default, the command line is echoed to standard error (stderr).
        /// </summary>
        /// <param name="name">The name of the command. This can be a path to an executable file.</param>
        /// <param name="args">The arguments to pass to the command.</param>
        /// <param name="workingDirectory">The working directory in which to run the command.</param>
        /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="windowsName">The name of the command to use on Windows only.</param>
        /// <param name="windowsArgs">The arguments to pass to the command on Windows only.</param>
        /// <param name="logPrefix">The prefix to use when logging messages to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="handleExitCode">
        /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
        /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
        /// returns <see langword="false"/> otherwise.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
        /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task RunAsync(
            string name,
            string args = null,
            string workingDirectory = null,
            bool noEcho = false,
            string windowsName = null,
            string windowsArgs = null,
            string logPrefix = null,
            Action<IDictionary<string, string>> configureEnvironment = null,
            bool createNoWindow = false,
            Func<int, bool> handleExitCode = null,
            CancellationToken cancellationToken = default)
        {
            Validate(name);

            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(
                    name,
                    args,
                    workingDirectory,
                    false,
                    windowsName,
                    windowsArgs,
                    configureEnvironment,
                    createNoWindow,
                    null);

                await process.RunAsync(noEcho, logPrefix ?? DefaultPrefix.Value, cancellationToken).ConfigureAwait(false);

                if (!(handleExitCode?.Invoke(process.ExitCode) ?? false) && process.ExitCode != 0)
                {
                    process.Throw();
                }
            }
        }

        /// <summary>
        /// Runs a command and reads standard output (stdout).
        /// By default, the command line is echoed to standard error (stderr).
        /// </summary>
        /// <param name="name">The name of the command. This can be a path to an executable file.</param>
        /// <param name="args">The arguments to pass to the command.</param>
        /// <param name="workingDirectory">The working directory in which to run the command.</param>
        /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="windowsName">The name of the command to use on Windows only.</param>
        /// <param name="windowsArgs">The arguments to pass to the command on Windows only.</param>
        /// <param name="logPrefix">The prefix to use when logging messages to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="encoding">The preferred <see cref="Encoding"/> for standard output (stdout).</param>
        /// <param name="handleExitCode">
        /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
        /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
        /// returns <see langword="false"/> otherwise.
        /// </param>
        /// <returns>A <see cref="string"/> representing the contents of standard output (stdout).</returns>
        /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        ///
        /// This method uses <see cref="Task.WaitAll(Task[])" /> and <see cref="System.Runtime.CompilerServices.TaskAwaiter.GetResult()"/>.
        /// This should be fine in most contexts, such as console apps, but in some contexts, such as a UI or ASP.NET, it may deadlock.
        /// In those contexts, <see cref="ReadAsync(string, string, string, bool, string, string, string, Action{IDictionary{string, string}}, bool, Encoding, Func{int, bool}, CancellationToken)" /> should be used instead.
        /// </remarks>
        public static string Read(
            string name,
            string args = null,
            string workingDirectory = null,
            bool noEcho = false,
            string windowsName = null,
            string windowsArgs = null,
            string logPrefix = null,
            Action<IDictionary<string, string>> configureEnvironment = null,
            bool createNoWindow = false,
            Encoding encoding = null,
            Func<int, bool> handleExitCode = null)
        {
            Validate(name);

            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(
                    name,
                    args,
                    workingDirectory,
                    true,
                    windowsName,
                    windowsArgs,
                    configureEnvironment,
                    createNoWindow,
                    encoding);

                var runProcess = process.RunAsync(noEcho, logPrefix ?? DefaultPrefix.Value, CancellationToken.None);

                Task<string> readOutput;
                try
                {
                    readOutput = process.StandardOutput.ReadToEndAsync();
                }
                catch (Exception)
                {
                    runProcess.GetAwaiter().GetResult();
                    throw;
                }

                Task.WaitAll(runProcess, readOutput);

                if (!(handleExitCode?.Invoke(process.ExitCode) ?? false) && process.ExitCode != 0)
                {
                    process.Throw();
                }

                return readOutput.Result;
            }
        }

        /// <summary>
        /// Runs a command and reads standard output (stdout).
        /// By default, the command line is echoed to standard error (stderr).
        /// </summary>
        /// <param name="name">The name of the command. This can be a path to an executable file.</param>
        /// <param name="args">The arguments to pass to the command.</param>
        /// <param name="workingDirectory">The working directory in which to run the command.</param>
        /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="windowsName">The name of the command to use on Windows only.</param>
        /// <param name="windowsArgs">The arguments to pass to the command on Windows only.</param>
        /// <param name="logPrefix">The prefix to use when logging messages to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="encoding">The preferred <see cref="Encoding"/> for standard output (stdout).</param>
        /// <param name="handleExitCode">
        /// A delegate which accepts an <see cref="int"/> representing exit code of the command and
        /// returns <see langword="true"/> when it has handled the exit code and default exit code handling should be suppressed, and
        /// returns <see langword="false"/> otherwise.
        /// </param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous running of the command and reading of standard output (stdout).
        /// The task result is a <see cref="string"/> representing the contents of standard output (stdout).
        /// </returns>
        /// <exception cref="ExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task<string> ReadAsync(
            string name,
            string args = null,
            string workingDirectory = null,
            bool noEcho = false,
            string windowsName = null,
            string windowsArgs = null,
            string logPrefix = null,
            Action<IDictionary<string, string>> configureEnvironment = null,
            bool createNoWindow = false,
            Encoding encoding = null,
            Func<int, bool> handleExitCode = null,
            CancellationToken cancellationToken = default)
        {
            Validate(name);

            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(
                    name,
                    args,
                    workingDirectory,
                    true,
                    windowsName,
                    windowsArgs,
                    configureEnvironment,
                    createNoWindow,
                    encoding);

                var runProcess = process.RunAsync(noEcho, logPrefix ?? DefaultPrefix.Value, cancellationToken);

                Task<string> readOutput;
                try
                {
                    readOutput = process.StandardOutput.ReadToEndAsync();
                }
                catch (Exception)
                {
                    await runProcess.ConfigureAwait(false);
                    throw;
                }

                await Task.WhenAll(runProcess, readOutput).ConfigureAwait(false);

                if (!(handleExitCode?.Invoke(process.ExitCode) ?? false) && process.ExitCode != 0)
                {
                    process.Throw();
                }

                return readOutput.Result;
            }
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
