namespace SimpleExec
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho, string echoPrefix)
        {
            process.EchoAndStart(noEcho, echoPrefix);
            process.WaitForExit();
        }

        public static Task RunAsync(this Process process, bool noEcho, string echoPrefix)
        {
            var tcs = new TaskCompletionSource<object>();
            process.Exited += (s, e) => tcs.SetResult(default);
            process.EnableRaisingEvents = true;
            process.EchoAndStart(noEcho, echoPrefix);
            return tcs.Task;
        }

        private static void EchoAndStart(this Process process, bool noEcho, string echoPrefix)
        {
            if (!noEcho)
            {
                var message = $"{(process.StartInfo.WorkingDirectory == "" ? "" : $"{echoPrefix}: Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{echoPrefix}: {process.StartInfo.FileName} {process.StartInfo.Arguments}";
                Console.Error.WriteLine(message);
            }

            process.Start();
        }

        public static void Throw(this Process process) =>
            throw new NonZeroExitCodeException(process.ExitCode);
    }
}
