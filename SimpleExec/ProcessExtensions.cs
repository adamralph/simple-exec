namespace SimpleExec
{
    using System;
    using System.Text;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class ProcessExtensions
    {
        public static string Run(this Process process, bool noEcho, string echoPrefix, CancellationToken cancellationToken)
        {
            using (var resetEvent = new ManualResetEventSlim(false))
            {
                StringBuilder stringBuilder = null;
                if (process.StartInfo.RedirectStandardOutput)
                {
                    stringBuilder = new StringBuilder();
                    process.OutputDataReceived += (s, e) => stringBuilder.AppendLine(e.Data);
                }

                process.Exited += (s, e) => resetEvent.SafeSet();
                process.EnableRaisingEvents = true;
                process.EchoAndStart(noEcho, echoPrefix);

                if (process.StartInfo.RedirectStandardOutput)
                {
                    process.BeginOutputReadLine();
                }

                try
                {
                    resetEvent.Wait(cancellationToken);
                    return stringBuilder?.ToString();
                }
                catch (OperationCanceledException)
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

        public static async Task<string> RunAsync(this Process process, bool noEcho, string echoPrefix, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>(TaskCreationOptions.RunContinuationsAsynchronously);

            using (cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken), useSynchronizationContext: false))
            {
                StringBuilder stringBuilder = null;
                if (process.StartInfo.RedirectStandardOutput)
                {
                    stringBuilder = new StringBuilder();
                    process.OutputDataReceived += (s, e) => stringBuilder.AppendLine(e.Data);
                }

                process.Exited += (s, e) => tcs.TrySetResult(default);
                process.EnableRaisingEvents = true;
                process.EchoAndStart(noEcho, echoPrefix);

                if (process.StartInfo.RedirectStandardOutput)
                {
                    process.BeginOutputReadLine();
                }

                try
                {
                    await tcs.Task.ConfigureAwait(false);
                }
                catch (OperationCanceledException)
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
                var message = $"{(string.IsNullOrEmpty(process.StartInfo.WorkingDirectory) ? "" : $"{echoPrefix}: Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{echoPrefix}: {process.StartInfo.FileName} {process.StartInfo.Arguments}";
                Console.Error.WriteLine(message);
            }

            process.Start();
        }

        public static void Throw(this Process process) =>
            throw new NonZeroExitCodeException(process.ExitCode);

        private static void SafeSet(this ManualResetEventSlim syncEvent)
        {
            try
            {
                syncEvent.Set();
            }
            catch (ObjectDisposedException)
            {
                // intentionally ignored
            }
        }
    }
}
