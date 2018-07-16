namespace SimpleExec
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    public static class Command
    {
        public static void Run(string name, string args) => Run(name, args, "");

        public static Task RunAsync(string name, string args) => RunAsync(name, args, "");

        public static void Run(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, false);
                process.Run();

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }
            }
        }

        public static async Task RunAsync(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, false);
                await process.RunAsync();

                if (process.ExitCode != 0)
                {
                    await process.ThrowAsync();
                }
            }
        }

        public static string Read(string name, string args) => Read(name, args, "");

        public static Task<string> ReadAsync(string name, string args) => ReadAsync(name, args, "");

        public static string Read(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, true);
                process.Run();

                if (process.ExitCode != 0)
                {
                    process.Throw();
                }

                return process.StandardOutput.ReadToEnd();
            }
        }

        public static async Task<string> ReadAsync(string name, string args, string workingDirectory)
        {
            using (var process = new Process())
            {
                process.StartInfo = CreateProcessInfo(name, args, workingDirectory, true);
                await process.RunAsync();

                if (process.ExitCode != 0)
                {
                    await process.ThrowAsync();
                }

                return await process.StandardOutput.ReadToEndAsync();
            }
        }

        private static ProcessStartInfo CreateProcessInfo(string name, string args, string workingDirectory, bool captureOutput) =>
            new ProcessStartInfo
            {
                FileName = name,
                Arguments = args,
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = captureOutput
            };
    }
}
