using Fixtures.Xunit;
using PublicApiGenerator;
using Xunit;

namespace Tests;

public static class PublicApi
{
    [Fact]
    public static async Task Surface()
    {
        var options = new ApiGeneratorOptions { IncludeAssemblyAttributes = false };

        var publicApi = typeof(SimpleExec.Command).Assembly.GeneratePublicApi(options);

        await Assert.ExpectedAsync(publicApi);
    }
}
