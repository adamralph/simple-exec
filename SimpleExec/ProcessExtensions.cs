namespace SimpleExec
{
    using System;
    using System.ComponentModel;
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
            var registration = cancellationToken.Register(
                state =>
                {
                    var currentProcess = (Process)state;
                    if (!currentProcess.HasExited)
                    {
                        try
                        {
                            currentProcess.Kill();
                        }

                        // Eat the exception, because the process has already exited.
                        catch (Win32Exception) { }
                        catch (InvalidOperationException) { }
                    }
                },
                process
            );

            // Attempt to dispose of the registration before the token is canceled
            // to help prevent unnecessary exception handling.
            process.Exited += (_, __) => registration.Dispose();
            return registration;
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
