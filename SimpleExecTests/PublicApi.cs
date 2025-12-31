using SimpleExec;
using PublicApiGenerator;
using Xunit;

namespace SimpleExecTests;

public static class PublicApi
{
    [Fact]
    public static async Task IsVerified()
    {
        var options = new ApiGeneratorOptions { IncludeAssemblyAttributes = false };

        var publicApi = typeof(Command).Assembly.GeneratePublicApi(options);

        _ = await Verify(publicApi);
    }
}
