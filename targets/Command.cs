using System.Diagnostics;
using System.Reflection;

namespace Targets;

internal static class Command
{
    private static readonly string EchoPrefix = Assembly.GetExecutingAssembly().GetName().Name!;

    internal static async Task RunAsync(string name, string args)
    {
        await Console.Error.WriteLineAsync($"{EchoPrefix}: {name} {args}");
        var process = Process.Start(name, args);
        await process.WaitForExitAsync();
        if (process.ExitCode != 0)
        {
            throw new ExitCodeException($"The command exited with code {process.ExitCode}.");
        }
    }

    internal static readonly Type ExitCodeExceptionType = typeof(ExitCodeException);

#pragma warning disable CA1032 // Implement standard exception constructors
    private sealed class ExitCodeException(string message) : Exception(message)
#pragma warning restore CA1032
    {
    }
}
