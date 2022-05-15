using System;
using System.IO;

namespace SimpleExecTests.Infra
{
    internal static class Capture
    {
#if NET6_0_OR_GREATER
        private static readonly Lazy<TextWriter> @out = new(() => new StringWriter());
#else
        private static readonly Lazy<TextWriter> @out = new Lazy<TextWriter>(() => new StringWriter());
#endif

        public static TextWriter Out => @out.Value;
    }
}
