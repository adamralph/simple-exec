namespace SimpleExec
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    internal static class ProcessExtensions
    {
        public static void Run(this Process process)
        {
            process.EchoAndStart();
            process.WaitForExit();
        }

        public static async Task RunAsync(this Process process)
        {
            process.EnableRaisingEvents = true;
            var tcs = new TaskCompletionSource<object>();
            process.Exited += (s, e) => tcs.SetResult(null);
            process.EchoAndStart();
            await tcs.Task.ConfigureAwait(false);
        }

        private static void EchoAndStart(this Process process)
        {
            var message = $"{(process.StartInfo.WorkingDirectory == "" ? "" : $"Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{process.StartInfo.FileName} {process.StartInfo.Arguments}";
            Console.Out.WriteLine(message);
            process.Start();
        }

        public static void Throw(this Process process) =>
            process.Throw(process.StandardError.ReadToEnd());

        public static async Task ThrowAsync(this Process process) =>
            process.Throw(await process.StandardError.ReadToEndAsync());

        private static void Throw(this Process process, string stdErr) =>
            throw new Exception($"The process exited with code {process.ExitCode}: {stdErr.Trim()}");
    }
}
