namespace SimpleExec
{
    using System.Threading.Tasks;

    public static partial class Command
    {
        public static void Run(string name, string args) => Run(name, args, "", false);

        public static void Run(string name, string args, bool noEcho) => Run(name, args, "", noEcho);

        public static void Run(string name, string args, string workingDirectory) => Run(name, args, workingDirectory, false);

        public static Task RunAsync(string name, string args) => RunAsync(name, args, "", false);

        public static Task RunAsync(string name, string args, bool noEcho) => RunAsync(name, args, "", noEcho);

        public static Task RunAsync(string name, string args, string workingDirectory) => RunAsync(name, args, workingDirectory, false);

        public static string Read(string name, string args) => Read(name, args, "", false);

        public static string Read(string name, string args, bool noEcho) => Read(name, args, "", noEcho);

        public static string Read(string name, string args, string workingDirectory) => Read(name, args, workingDirectory, false);

        public static Task<string> ReadAsync(string name, string args) => ReadAsync(name, args, "", false);

        public static Task<string> ReadAsync(string name, string args, bool noEcho) => ReadAsync(name, args, "", noEcho);

        public static Task<string> ReadAsync(string name, string args, string workingDirectory) => ReadAsync(name, args, workingDirectory, false);
    }
}
