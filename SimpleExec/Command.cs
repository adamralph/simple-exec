using System.IO;

namespace SimpleExec
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    public delegate void OutputHandler(string s);

    /// <summary>
    /// Contains methods for running commands and reading standard output (stdout).
    /// </summary>
    public static partial class Command
    {
        /// <summary>
        /// Runs a command.
        /// By default, the command line is echoed to standard error (stderr).
        /// </summary>
        /// <param name="name">The name of the command. This can be a path to an executable file.</param>
        /// <param name="args">The arguments to pass to the command.</param>
        /// <param name="workingDirectory">The working directory in which to run the command.</param>
        /// <param name="noEcho">Whether or not to echo the resulting command line and working directory (if specified) to standard error (stderr).</param>
        /// <param name="stdOut">Gives you access to the StandardOutput string</param>
        /// <param name="stdErr">Gives you access to the StandardError string</param>
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static void Run(string name, string args = null, string workingDirectory = null, bool noEcho = false,
            OutputHandler stdOut = null, OutputHandler stdErr = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, false);
                process.Run(noEcho);

                stdOut?.Invoke(process.StandardOutput.ReadToEnd());
                stdErr?.Invoke(process.StandardError.ReadToEnd());

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
        /// <param name="stdOut">Gives you access to the StandardOutput string</param>
        /// <param name="stdErr">Gives you access to the StandardError string</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous running of the command.</returns>
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task RunAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false,
            OutputHandler stdOut = null, OutputHandler stdErr = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, false);
                await process.RunAsync(noEcho).ConfigureAwait(false);

                stdOut?.Invoke(await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false));
                stdErr?.Invoke(await process.StandardError.ReadToEndAsync().ConfigureAwait(false));

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
        /// <param name="stdOut">Gives you access to the StandardOutput string</param>
        /// <param name="stdErr">Gives you access to the StandardError string</param>
        /// <returns>A <see cref="string"/> representing the contents of standard output (stdout).</returns>
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static string Read(string name, string args = null, string workingDirectory = null, bool noEcho = false,
            OutputHandler stdOut = null, OutputHandler stdErr = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true);
                process.Run(noEcho);

                var output = process.StandardOutput.ReadToEnd();

                stdOut?.Invoke(output);
                stdErr?.Invoke(process.StandardError.ReadToEnd());

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
        /// <param name="stdOut">Gives you access to the StandardOutput string</param>
        /// <param name="stdErr">Gives you access to the StandardError string</param>
        /// <returns>
        /// A <see cref="Task{string}"/> representing the asynchronous running of the command and reading of standard output (stdout).
        /// The task result contains the contents of standard output (stdout).
        /// </returns>
        /// <exception cref="CommandException">The command exited with non-zero exit code.</exception>
        /// <remarks>
        /// By default, the resulting command line and the working directory (if specified) are echoed to standard error (stderr).
        /// To suppress this behavior, provide the <paramref name="noEcho"/> parameter with a value of <c>true</c>.
        /// </remarks>
        public static async Task<string> ReadAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false,
            OutputHandler stdOut = null, OutputHandler stdErr = null)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true);
                await process.RunAsync(noEcho).ConfigureAwait(false);

                var output = await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);

                stdOut?.Invoke(output);
                stdErr?.Invoke(await process.StandardError.ReadToEndAsync().ConfigureAwait(false));

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return output;
            }
        }
    }
}
