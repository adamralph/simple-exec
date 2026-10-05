using Fixtures.Xunit;
using PublicApiGenerator;
using SimpleExec;
using Xunit;

namespace Tests;

public static class PublicApi
{
    [Fact]
    public static async Task Surface()
    {
        var options = new ApiGeneratorOptions { IncludeAssemblyAttributes = false };

        var publicApi = typeof(Command).Assembly.GeneratePublicApi(options);

        await Assert.ExpectedAsync(publicApi);
    }
}
