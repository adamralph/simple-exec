using System;
using System.Diagnostics;
using System.Text;
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
                Console.Out.Write(process.StartInfo.GetEchoLines(echoPrefix));
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
                await Console.Out.WriteAsync(process.StartInfo.GetEchoLines(echoPrefix)).ConfigureAwait(false);
            }

            _ = process.Start();

            await using (cancellationToken.Register(
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

        private static string GetEchoLines(this System.Diagnostics.ProcessStartInfo info, string echoPrefix)
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrEmpty(info.WorkingDirectory))
            {
                _ = builder.AppendLine($"{echoPrefix}: Working directory: {info.WorkingDirectory}");
            }

            if (info.ArgumentList.Count > 0)
            {
                _ = builder.AppendLine($"{echoPrefix}: {info.FileName}");

                foreach (var arg in info.ArgumentList)
                {
                    _ = builder.AppendLine($"{echoPrefix}:   {arg}");
                }
            }
            else
            {
                _ = builder.AppendLine($"{echoPrefix}: {info.FileName}{(string.IsNullOrEmpty(info.Arguments) ? "" : $" {info.Arguments}")}");
            }

            return builder.ToString();
        }

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
