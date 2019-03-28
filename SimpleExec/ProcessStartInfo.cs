namespace SimpleExec
{
    using System.Runtime.InteropServices;

    internal static class ProcessStartInfo
    {
        public static System.Diagnostics.ProcessStartInfo Create(
            string name, string args, string workingDirectory, bool captureOutput) =>
            (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ? new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = !string.IsNullOrEmpty(args) ? $"/c \"{name} {args}\"" : $"/c \"{name}\"",
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = captureOutput
                }
                : new System.Diagnostics.ProcessStartInfo
                {
                    FileName = name,
                    Arguments = args,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = captureOutput
                };
    }
}
