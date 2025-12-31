using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class ConfiguringEnvironments
{
    [Fact]
    public static async Task ConfiguringEnvironment()
    {
        // act
        var (standardOutput, _) = await Command.ReadAsync(
            "dotnet",
            $"exec {Tester.Path} environment",
            configureEnvironment: env => env["foo"] = "bar",
            ct: TestContext.Current.CancellationToken);

        // assert
        Assert.Contains("foo=bar", standardOutput, StringComparison.Ordinal);
    }
}
