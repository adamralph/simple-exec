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

        public static void EchoAndStart(this Process process, bool noEcho)
        {
            if (!noEcho)
            {
                var message = $"{(process.StartInfo.WorkingDirectory == "" ? "" : $"Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{process.StartInfo.FileName} {process.StartInfo.Arguments}";
                Console.Error.WriteLine(message);
            }

            process.Start();
        }

        public static void Throw(this Process process) =>
            throw new NonZeroExitCodeException(process.ExitCode);
    }
}
