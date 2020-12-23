using System;
using System.IO;

namespace SimpleExecTests.Infra
{
    internal static class Capture
    {
        private static readonly Lazy<TextWriter> error = new Lazy<TextWriter>(() => new StringWriter());

        public static TextWriter Error => error.Value;
    }
}
