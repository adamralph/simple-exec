using System.Diagnostics;
using SimpleExec;
using Xunit;
using static SimpleExec.Command;

namespace SimpleExecTests;

public static class OpenApiChanges
{
    private static Ct Ct => TestContext.Current.CancellationToken;

    [Fact]
    public static async Task RunningOpenApiChanges()
    {
        const string name = @"C:\Users\adam\Downloads\openapi-changes_0.2.6_windows_x86_64\openapi-changes.exe";

        // Command.Run(name, ct: TestContext.Current.CancellationToken);
        // await Command.RunAsync(name, ct: TestContext.Current.CancellationToken);
        var (standardOutput, _) = await Command.ReadAsync(name,configureEnvironment: env =>
        {
            env["PB33F_DARK_BACKGROUND"] = "1";
        });

var (standardOutput1, _) = await ReadAsync(
    @"C:\temp\openapi-changes.exe",
    configureEnvironment: env => env["PB33F_DARK_BACKGROUND"] = "1");

        Assert.NotEmpty(standardOutput);
    }

    [Fact]
    public static async Task Foo()
    {

        var psi = new ProcessStartInfo(@"C:\Users\adam\Downloads\openapi-changes.exe")
        {
            // RedirectStandardOutput = true,
            // RedirectStandardError = true,
            UseShellExecute = false,
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        // var stdoutTask = process.StandardOutput.ReadToEndAsync();

        process.WaitForExit();

        // string stdout = await stdoutTask;

        // Console.WriteLine(stdout);
    }
}
