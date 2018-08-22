namespace SimpleExec
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho)
        {
            process.EchoAndStart(noEcho);
            process.WaitForExit();
        }

        public static async Task RunAsync(this Process process, bool noEcho)
        {
            process.EnableRaisingEvents = true;
            var tcs = new TaskCompletionSource<object>();
            process.Exited += (s, e) => tcs.SetResult(null);
            process.EchoAndStart(noEcho);
            await tcs.Task.ConfigureAwait(false);
        }

        private static void EchoAndStart(this Process process, bool noEcho)
        {
            if (!noEcho)
            {
                var message = $"{(process.StartInfo.WorkingDirectory == "" ? "" : $"Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{process.StartInfo.FileName} {process.StartInfo.Arguments}";
                Console.Out.WriteLine(message);
            }

            process.Start();
        }

        public static void Throw(this Process process) =>
            process.Throw(process.StandardError.ReadToEnd());

        public static async Task ThrowAsync(this Process process) =>
            process.Throw(await process.StandardError.ReadToEndAsync().ConfigureAwait(false));

        private static void Throw(this Process process, string stdErr) =>
            throw new Exception($"The process exited with code {process.ExitCode}: {stdErr.Trim()}");
    }
}
