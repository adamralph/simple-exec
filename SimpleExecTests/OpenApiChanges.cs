using SimpleExec;
using Xunit;

namespace SimpleExecTests;

public static class OpenApiChanges
{
    private static Ct Ct => TestContext.Current.CancellationToken;

    [Fact]
    public static async Task RunningOpenApiChanges()
    {
        const string name = "/Users/adam/Downloads/openapi-changes_0.2.5_darwin_arm64/openapi-changes";

        var (standardOutput, _) = await Command.ReadAsync(name, ct: TestContext.Current.CancellationToken);

        Assert.NotEmpty(standardOutput);
    }
}
