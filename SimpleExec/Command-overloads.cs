namespace SimpleExec
{
    using System.Threading.Tasks;

    public static partial class Command
    {
        public static void Run(string name, string args) => Run(name, args, "");

        public static Task RunAsync(string name, string args) => RunAsync(name, args, "");

        public static string Read(string name, string args) => Read(name, args, "");

        public static Task<string> ReadAsync(string name, string args) => ReadAsync(name, args, "");
    }
}
