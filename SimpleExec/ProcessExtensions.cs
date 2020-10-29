namespace SimpleExec
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho, string echoPrefix)
        {
            process.EchoAndStart(noEcho, echoPrefix);
            process.WaitForExit();
        }

        public static async Task RunAsync(this Process process, bool noEcho, string echoPrefix, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();

            using (var started = new SemaphoreSlim(0, 1))
            using (var ranToCompletionOrCanceled = new SemaphoreSlim(1, 1))
            using (var cancellation = cancellationToken.Register(() =>
            {
                if (CatchDisposed(() => started.Wait()))
                {
                    return;
                }

                if (tcs.Task.Status == TaskStatus.RanToCompletion)
                {
                    return;
                }

                if (CatchDisposed(() => ranToCompletionOrCanceled.Wait()))
                {
                    return;
                }

                try
                {
                    if (tcs.Task.Status == TaskStatus.RanToCompletion)
                    {
                        return;
                    }

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

                    // SetCanceled(cancellationToken) would make more sense here
                    // but it only exists from .NET 5 onwards
                    // see https://github.com/dotnet/runtime/issues/30862
                    // however, we know TrySetCanceled will succeed
                    // because we have full control of the underlying task
                    tcs.TrySetCanceled(cancellationToken);
                }
                finally
                {
                    CatchDisposed(() => ranToCompletionOrCanceled.Release());
                }
            }))
            {
                process.Exited += (s, e) =>
                {
                    if (CatchDisposed(() => cancellation.Dispose()))
                    {
                        return;
                    }

                    if (tcs.Task.Status == TaskStatus.Canceled)
                    {
                        return;
                    }

                    if (CatchDisposed(() => ranToCompletionOrCanceled.Wait()))
                    {
                        return;
                    }

                    try
                    {
                        if (tcs.Task.Status == TaskStatus.Canceled)
                        {
                            return;
                        }

                        tcs.SetResult(default);
                    }
                    finally
                    {
                        CatchDisposed(() => ranToCompletionOrCanceled.Release());
                    }
                };

                process.EnableRaisingEvents = true;
                process.EchoAndStart(noEcho, echoPrefix);
                started.Release();

                await tcs.Task.ConfigureAwait(false);
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

        private static bool CatchDisposed(Action action)
        {
            try
            {
                action();
            }
            catch (ObjectDisposedException)
            {
                return true;
            }

            return false;
        }
    }
}
