using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleExec
{
    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho, string echoPrefix, CancellationToken cancellationToken)
        {
            var cancelled = 0L;

            if (!noEcho)
            {
                Console.Out.WriteLine(process.GetMessage(echoPrefix));
            }

            _ = process.Start();

            using (cancellationToken.Register(
                () =>
                {
                    if (process.TryKill())
                    {
                        _ = Interlocked.Increment(ref cancelled);
                    }
                },
                useSynchronizationContext: false))
            {
                process.WaitForExit();
            }

            if (Interlocked.Read(ref cancelled) == 1)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        public static async Task RunAsync(this Process process, bool noEcho, string echoPrefix, CancellationToken cancellationToken)
        {
            // NOTE: can switch to TaskCompletionSource when moving to .NET 5+
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            process.EnableRaisingEvents = true;
            process.Exited += (s, e) => tcs.TrySetResult(0);

            if (!noEcho)
            {
                await Console.Out.WriteLineAsync(process.GetMessage(echoPrefix)).ConfigureAwait(false);
            }

            _ = process.Start();

            using (cancellationToken.Register(
                () =>
                {
                    if (process.TryKill())
                    {
                        _ = tcs.TrySetCanceled(cancellationToken);
                    }
                },
                useSynchronizationContext: false))
            {
                _ = await tcs.Task.ConfigureAwait(false);
            }
        }

        private static string GetMessage(this Process process, string echoPrefix) =>
            $"{(string.IsNullOrEmpty(process.StartInfo.WorkingDirectory) ? "" : $"{echoPrefix}: Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{echoPrefix}: {process.StartInfo.FileName} {process.StartInfo.Arguments}";

        private static bool TryKill(this Process process)
        {
            // exceptions may be thrown for all kinds of reasons
            // and the _same exception_ may be thrown for all kinds of reasons
            // System.Diagnostics.Process is "fine"
            try
            {
                process.Kill();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return false;
            }

            return true;
        }
    }
}
