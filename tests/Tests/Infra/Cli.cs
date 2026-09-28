namespace Tests.Infra;

internal static class Cli
{
    public static string Path =>
#if NET8_0 && DEBUG
        "../../../../../fixtures/Fixtures.Cli/bin/Debug/net8.0/Fixtures.Cli.dll";
#endif
#if NET8_0 && RELEASE
        "../../../../../fixtures/Fixtures.Cli/bin/Release/net8.0/Fixtures.Cli.dll";
#endif
#if NET9_0 && DEBUG
        "../../../../../fixtures/Fixtures.Cli/bin/Debug/net9.0/Fixtures.Cli.dll";
#endif
#if NET9_0 && RELEASE
        "../../../../../fixtures/Fixtures.Cli/bin/Release/net9.0/Fixtures.Cli.dll";
#endif
#if NET10_0 && DEBUG
        "../../../../../fixtures/Fixtures.Cli/bin/Debug/net10.0/Fixtures.Cli.dll";
#endif
#if NET10_0 && RELEASE
        "../../../../../fixtures/Fixtures.Cli/bin/Release/net10.0/Fixtures.Cli.dll";
#endif
}
