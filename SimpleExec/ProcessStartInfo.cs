namespace SimpleExec
{
    using System.Runtime.InteropServices;

    internal static class ProcessStartInfo
    {
        public static System.Diagnostics.ProcessStartInfo Create(
            string name, string args, string workingDirectory, bool captureOutput, bool captureError, string windowsName, string windowsArgs) =>
            (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ? new System.Diagnostics.ProcessStartInfo
                {
                    FileName = windowsName ?? name,
                    Arguments = windowsArgs ?? args,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = captureError,
                    RedirectStandardOutput = captureOutput
                }
                : new System.Diagnostics.ProcessStartInfo
                {
                    FileName = name,
                    Arguments = args,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = captureError,
                    RedirectStandardOutput = captureOutput
                };
    }
}
