using SimpleExec;
using Tests.Infra;
using Xunit;

namespace Tests;

public static class ConfiguringEnvironments
{
    [Fact]
    public static async Task ConfiguringEnvironment()
    {
        // act
        var (standardOutput, _) = await Command.ReadAsync(
            "dotnet",
            $"exec {Cli.Path} environment",
            configureEnvironment: env => env["foo"] = "bar",
            ct: TestContext.Current.CancellationToken);

        // assert
        Assert.Contains("foo=bar", standardOutput, StringComparison.Ordinal);
    }
}
