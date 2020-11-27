namespace SimpleExec
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

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
        /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <exception cref="NonZeroExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static void Run(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null, string echoPrefix = null, Action<IDictionary<string, string>> configureEnvironment = null, bool createNoWindow = false, CancellationToken cancellationToken = default)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, false, windowsName, windowsArgs, configureEnvironment, createNoWindow);
                _ = process.Run(noEcho, echoPrefix ?? DefaultPrefix.Value, cancellationToken);

                if (process.ExitCode != 0)
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
        /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
        /// <exception cref="NonZeroExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task RunAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null, string echoPrefix = null, Action<IDictionary<string, string>> configureEnvironment = null, bool createNoWindow = false, CancellationToken cancellationToken = default)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, false, windowsName, windowsArgs, configureEnvironment, createNoWindow);
                await process.RunAsync(noEcho, echoPrefix ?? DefaultPrefix.Value, cancellationToken).ConfigureAwait(false);

                if (process.ExitCode != 0)
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
        /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <returns>A <see cref="string"/> representing the contents of standard output (stdout).</returns>
        /// <exception cref="NonZeroExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static string Read(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null, string echoPrefix = null, Action<IDictionary<string, string>> configureEnvironment = null, bool createNoWindow = false, CancellationToken cancellationToken = default)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true, windowsName, windowsArgs, configureEnvironment, createNoWindow);
                var output = process.Run(noEcho, echoPrefix ?? DefaultPrefix.Value, cancellationToken);

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return output;
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
        /// <param name="echoPrefix">The prefix to use when echoing the command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="configureEnvironment">An action which configures environment variables for the command.</param>
        /// <param name="createNoWindow">Whether to run the command in a new window.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the command to exit.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous running of the command and reading of standard output (stdout).
        /// The task result is a <see cref="string"/> representing the contents of standard output (stdout).
        /// </returns>
        /// <exception cref="NonZeroExitCodeException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task<string> ReadAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null, string echoPrefix = null, Action<IDictionary<string, string>> configureEnvironment = null, bool createNoWindow = false, CancellationToken cancellationToken = default)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true, windowsName, windowsArgs, configureEnvironment, createNoWindow);

                var runProcess = process.RunAsync(noEcho, echoPrefix ?? DefaultPrefix.Value, cancellationToken);
                var readOutput = process.StandardOutput.ReadToEndAsync();

                await Task.WhenAll(runProcess, readOutput).ConfigureAwait(false);

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return readOutput.Result;
            }
        }
    }
}
