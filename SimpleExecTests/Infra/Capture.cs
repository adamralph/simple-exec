namespace SimpleExecTests.Infra;

internal static class Capture
{
    private static readonly Lazy<TextWriter> LazyOut = new(() => new StringWriter());

    public static TextWriter Out => LazyOut.Value;
}
