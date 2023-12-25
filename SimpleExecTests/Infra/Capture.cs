using System;
using System.IO;

namespace SimpleExecTests.Infra;

internal static class Capture
{
    private static readonly Lazy<TextWriter> @out = new(() => new StringWriter());

    public static TextWriter Out => @out.Value;
}
