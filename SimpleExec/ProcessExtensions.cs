namespace SimpleExec
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho)
        {
            process.EchoAndStart(noEcho);
            process.WaitForExit();
        }

        public static async Task RunAsync(this Process process, bool noEcho, CancellationToken cancellationToken = default(CancellationToken))
        {
            process.EnableRaisingEvents = true;
            var tcs = new TaskCompletionSource<object>();
            process.Exited += (s, e) => tcs.SetResult(null);
            process.EchoAndStart(noEcho);
            using (KillProcessOnCancellation(process, cancellationToken))
            {
                await tcs.Task.ConfigureAwait(false);
            }
        }

        static CancellationTokenRegistration KillProcessOnCancellation(Process process, CancellationToken cancellationToken)
        {
            return cancellationToken.Register(
                state =>
                {
                    var currentProcess = (Process)state;
                    if (!currentProcess.HasExited)
                    {
                        currentProcess.Kill();
                    }
                },
                process
            );
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
            throw new Exception($"The process exited with code {process.ExitCode}.");
    }
}
