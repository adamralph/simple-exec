using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleExec
{
    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho, string logPrefix)
        {
            process.EchoAndStart(noEcho, logPrefix);
            process.WaitForExit();
        }

        public static async Task RunAsync(this Process process, bool noEcho, string logPrefix, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken), useSynchronizationContext: false))
            {
                process.Exited += (s, e) => tcs.TrySetResult(default);
                process.EnableRaisingEvents = true;
                await process.EchoAndStartAsync(noEcho, logPrefix).ConfigureAwait(false);

                try
                {
                    _ = await tcs.Task.ConfigureAwait(false);
                }
                catch (OperationCanceledException ex) when (ex.CancellationToken == cancellationToken)
                {
                    // best effort only, since exceptions may be thrown for all kinds of reasons
                    // and the _same exception_ may be thrown for all kinds of reasons
                    // System.Diagnostics.Process is "fine"
                    try
                    {
                        process.Kill();
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception killEx)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        await Console.Error.WriteLineAsync($"{logPrefix}: Exception thrown during cancellation: {killEx}").ConfigureAwait(false);
                    }

                    throw;
                }
            }
        }

        private static void EchoAndStart(this Process process, bool noEcho, string logPrefix)
        {
            if (!noEcho)
            {
                Console.Error.WriteLine(GetMessage(process, logPrefix));
            }

            _ = process.Start();
        }

        private static async Task EchoAndStartAsync(this Process process, bool noEcho, string logPrefix)
        {
            if (!noEcho)
            {
                await Console.Error.WriteLineAsync(GetMessage(process, logPrefix)).ConfigureAwait(false);
            }

            _ = process.Start();
        }

        private static string GetMessage(Process process, string logPrefix) =>
            $"{(string.IsNullOrEmpty(process.StartInfo.WorkingDirectory) ? "" : $"{logPrefix}: Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{logPrefix}: {process.StartInfo.FileName} {process.StartInfo.Arguments}";

        public static void Throw(this Process process) =>
            throw new ExitCodeException(process.ExitCode);
    }
}
