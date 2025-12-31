using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace SimpleExec;

internal static class ProcessExtensions
{
    public static void Run(this Process process, IEnumerable<string> secrets, bool noEcho, string echoPrefix, bool cancellationIgnoresProcessTree, CancellationToken cancellationToken)
    {
        var cancelled = new StrongBox<long>(0);

        if (!noEcho)
        {
            Console.Out.Write(process.StartInfo.GetEchoLines(secrets, echoPrefix));
        }

        _ = process.Start();

        using var register = cancellationToken.Register(
            () =>
            {
                if (process.TryKill(cancellationIgnoresProcessTree))
                {
                    _ = Interlocked.Increment(ref cancelled.Value);
                }
            },
            useSynchronizationContext: false);

        process.WaitForExit();

        if (Interlocked.Read(ref cancelled.Value) == 1)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    public static async Task RunAsync(this Process process, IEnumerable<string> secrets, bool noEcho, string echoPrefix, bool cancellationIgnoresProcessTree, CancellationToken cancellationToken)
    {
        using var sync = new SemaphoreSlim(1, 1);
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        process.EnableRaisingEvents = true;
        process.Exited += (_, _) => sync.Run(() => tcs.Task.Status != TaskStatus.Canceled, () => _ = tcs.TrySetResult());

        if (!noEcho)
        {
            await Console.Out.WriteAsync(process.StartInfo.GetEchoLines(secrets, echoPrefix)).ConfigureAwait(false);
        }

        _ = process.Start();

        await using var register = cancellationToken.Register(
            () => sync.Run(
                () => tcs.Task.Status != TaskStatus.RanToCompletion,
                () =>
                {
                    if (process.TryKill(cancellationIgnoresProcessTree))
                    {
                        _ = tcs.TrySetCanceled(cancellationToken);
                    }
                }),
            useSynchronizationContext: false).ConfigureAwait(false);

        await tcs.Task.ConfigureAwait(false);
    }

    private static string GetEchoLines(this System.Diagnostics.ProcessStartInfo info, IEnumerable<string> secrets, string echoPrefix)
    {
        var builder = new StringBuilder();

        if (!string.IsNullOrEmpty(info.WorkingDirectory))
        {
            _ = builder.AppendLine(CultureInfo.InvariantCulture, $"{echoPrefix}: Working directory: {info.WorkingDirectory}");
        }

        if (info.ArgumentList.Count > 0)
        {
            _ = builder.AppendLine(CultureInfo.InvariantCulture, $"{echoPrefix}: {info.FileName}");

            foreach (var arg in info.ArgumentList)
            {
                _ = builder.AppendLine(CultureInfo.InvariantCulture, $"{echoPrefix}:   {arg}");
            }
        }
        else
        {
            _ = builder.AppendLine(CultureInfo.InvariantCulture, $"{echoPrefix}: {info.FileName}{(string.IsNullOrEmpty(info.Arguments) ? "" : $" {info.Arguments}")}");
        }

        return builder.ToString().Redact(secrets);
    }

    private static string Redact(this string value, IEnumerable<string> secrets) =>
        secrets.Aggregate(value, (current, secret) => current.Replace(secret, "***", StringComparison.OrdinalIgnoreCase));

    private static bool TryKill(this Process process, bool ignoreProcessTree)
    {
        // exceptions may be thrown for all kinds of reasons
        // and the _same exception_ may be thrown for all kinds of reasons
        // System.Diagnostics.Process is "fine"
        try
        {
            process.Kill(!ignoreProcessTree);
        }
#pragma warning disable CA1031 // Do not catch general exception types
        catch (Exception)
#pragma warning restore CA1031
        {
            return false;
        }

        return true;
    }

    private static void Run(this SemaphoreSlim sync, Func<bool> doubleCheckPredicate, Action action)
    {
        if (!doubleCheckPredicate())
        {
            return;
        }

        try
        {
            sync.Wait();
        }
        catch (ObjectDisposedException)
        {
            return;
        }

        try
        {
            if (doubleCheckPredicate())
            {
                action();
            }
        }
        finally
        {
            try
            {
                _ = sync.Release();
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}
