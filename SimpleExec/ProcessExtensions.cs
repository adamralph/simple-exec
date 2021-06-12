using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleExec
{
    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho, string echoPrefix)
        {
            process.EchoAndStart(noEcho, echoPrefix);
            process.WaitForExit();
        }

        public static async Task RunAsync(this Process process, bool noEcho, string echoPrefix, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken), useSynchronizationContext: false))
            {
                process.Exited += (s, e) => tcs.TrySetResult(default);
                process.EnableRaisingEvents = true;
                await process.EchoAndStartAsync(noEcho, echoPrefix).ConfigureAwait(false);

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
                    catch
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                    }

                    throw;
                }
            }
        }

        private static void EchoAndStart(this Process process, bool noEcho, string echoPrefix)
        {
            if (!noEcho)
            {
                Console.Error.WriteLine(GetMessage(process, echoPrefix));
            }

            _ = process.Start();
        }

        private static async Task EchoAndStartAsync(this Process process, bool noEcho, string echoPrefix)
        {
            if (!noEcho)
            {
                await Console.Error.WriteLineAsync(GetMessage(process, echoPrefix)).ConfigureAwait(false);
            }

            _ = process.Start();
        }

        private static string GetMessage(Process process, string echoPrefix) =>
            $"{(string.IsNullOrEmpty(process.StartInfo.WorkingDirectory) ? "" : $"{echoPrefix}: Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{echoPrefix}: {process.StartInfo.FileName} {process.StartInfo.Arguments}";

        public static void Throw(this Process process) =>
            throw new ExitCodeException(process.ExitCode);
    }
}
