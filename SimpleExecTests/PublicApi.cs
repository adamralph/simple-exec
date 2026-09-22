using SimpleExec;
using PublicApiGenerator;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class PublicApi
{
    [Fact]
    public static async Task IsVerified()
    {
        var options = new ApiGeneratorOptions { IncludeAssemblyAttributes = false };

        var publicApi = typeof(Command).Assembly.GeneratePublicApi(options);

        await publicApi.Verify();
    }
}
