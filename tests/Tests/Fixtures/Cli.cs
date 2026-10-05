namespace Tests.Fixtures;

internal static class Cli
{
    public static string Path { get; } = System.IO.Path.Combine(AppContext.BaseDirectory, "Fixtures.Cli.dll");
}
