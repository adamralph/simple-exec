namespace SimpleExec
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    public static partial class Command
    {
        public static void Run(string name, string args = null, string workingDirectory = null, bool noEcho = false)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, false);
                process.Run(noEcho);

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }
            }
        }

        public static async Task RunAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, false);
                await process.RunAsync(noEcho).ConfigureAwait(false);

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }
            }
        }

        public static string Read(string name, string args = null, string workingDirectory = null, bool noEcho = false)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true);
                process.Run(noEcho);

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return process.StandardOutput.ReadToEnd();
            }
        }

        public static async Task<string> ReadAsync(string name, string args = null, string workingDirectory = null, bool noEcho = false)
        {
            using (var process = new Process())
            {
                process.StartInfo = ProcessStartInfo.Create(name, args, workingDirectory, true);
                await process.RunAsync(noEcho).ConfigureAwait(false);

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return await process.StandardOutput.ReadToEndAsync().ConfigureAwait(false);
            }
        }
    }
}
