namespace SimpleExec
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public class Command
    {
        private readonly string[] args;

        private Command(string name, params string[] args)
        {
            this.Name = name;
            this.args = args.ToArray();
        }

        public string Name { get; }

        public string[] Args => this.args;

        public string Dir { get; set; }

        public static void Run(string name, params string[] args) => new Command(name, args).Run();

        public static void RunIn(string dir, string name, params string[] args) => new Command(name, args) { Dir = dir }.Run();

        private void Run()
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = $"\"{this.Name}\"",
                    Arguments = string.Join(" ", this.Args),
                    UseShellExecute = false,
                    WorkingDirectory = this.Dir,
                };

                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new InvalidOperationException($"The command exited with code {process.ExitCode}.");
                }
            }
        }
    }
}
