using PublicApiGenerator;
using SimpleExec;
using Tests.Infra;
using Xunit;

namespace Tests;

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
