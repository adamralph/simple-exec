namespace SimpleExec
{
    using System;
    using System.Diagnostics;
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
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static void Run(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null, Action<string> outputDataReceived = null, Action<string> errorDataReceived = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, outputDataReceived!=null, errorDataReceived != null, windowsName, windowsArgs);
                process.Run(noEcho, outputDataReceived, errorDataReceived);

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
        /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task RunAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null, Action<string> outputDataReceived = null, Action<string> errorDataReceived = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, outputDataReceived != null, errorDataReceived != null, windowsName, windowsArgs);
                await process.RunAsync(noEcho, outputDataReceived, errorDataReceived).ConfigureAwait(false);

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
        /// <returns>A <see cref="string"/> representing the contents of standard output (stdout).</returns>
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static string Read(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true, false, windowsName, windowsArgs);

                var runProcess = process.RunAsync(noEcho, null, null);
                var readOutput = process.StandardOutput.ReadToEndAsync();

                Task.WaitAll(runProcess, readOutput);

                if (process.ExitCode != 0)
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
        /// <returns>
        /// A <see cref="Task{string}"/> representing the asynchronous running of the command and reading of standard output (stdout).
        /// The task result contains the contents of standard output (stdout).
        /// </returns>
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task<string> ReadAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false, string windowsName = null, string windowsArgs = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true, false, windowsName, windowsArgs);

                var runProcess = process.RunAsync(noEcho, null, null);
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
