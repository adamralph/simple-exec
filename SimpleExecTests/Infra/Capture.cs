using System;
using System.IO;

namespace SimpleExecTests.Infra
{
    internal static class Capture
    {
#if NET5_0_OR_GREATER
        private static readonly Lazy<TextWriter> error = new(() => new StringWriter());
#else
        private static readonly Lazy<TextWriter> error = new Lazy<TextWriter>(() => new StringWriter());
#endif

        public static TextWriter Error => error.Value;
    }
}
