namespace SimpleExec
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    internal static class ProcessExtensions
    {
        public static void Run(this Process process, bool noEcho, Action<string> outputDataReceived, Action<string> errorDataReceived)
        {
            if (outputDataReceived != null)
            {
                process.OutputDataReceived += (o, e) => outputDataReceived(e.Data);
                process.EnableRaisingEvents = true;
            }

            if (errorDataReceived != null)
            {
                process.ErrorDataReceived += (o, e) => errorDataReceived(e.Data);
                process.EnableRaisingEvents = true;
            }

            process.EchoAndStart(noEcho);

            if (outputDataReceived != null)
            {
                process.BeginOutputReadLine();
            }

            if (errorDataReceived != null)
            {
                process.BeginErrorReadLine();
            }

            process.WaitForExit();
        }

        public static Task RunAsync(this Process process, bool noEcho, Action<string> outputDataReceived, Action<string> errorDataReceived)
        {
            if (outputDataReceived != null)
            {
                process.OutputDataReceived += (o, e) => outputDataReceived(e.Data);
            }

            if (errorDataReceived != null)
            {
                process.ErrorDataReceived += (o, e) => errorDataReceived(e.Data);
            }

            var tcs = new TaskCompletionSource<object>();
            process.Exited += (s, e) => tcs.SetResult(null);
            process.EnableRaisingEvents = true;
            process.EchoAndStart(noEcho);

            if (outputDataReceived != null)
            {
                process.BeginOutputReadLine();
            }

            if (errorDataReceived != null)
            {
                process.BeginErrorReadLine();
            }

            return tcs.Task;
        }

        private static void EchoAndStart(this Process process, bool noEcho)
        {
            if (!noEcho)
            {
                var message = $"{(process.StartInfo.WorkingDirectory == "" ? "" : $"Working directory: {process.StartInfo.WorkingDirectory}{Environment.NewLine}")}{process.StartInfo.FileName} {process.StartInfo.Arguments}";
                Console.Error.WriteLine(message);
            }

            process.Start();
        }

        public static void Throw(this Process process) =>
            throw new NonZeroExitCodeException(process.ExitCode);
    }
}
