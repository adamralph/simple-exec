using System.Reflection;

namespace SimpleExec
{
    internal static class DefaultPrefix
    {
        static DefaultPrefix()
        {
        }

        public static readonly string Value = Assembly.GetEntryAssembly()?.GetName().Name ?? "SimpleExec";
    }
}
