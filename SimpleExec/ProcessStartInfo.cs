namespace SimpleExec
{
    using System.Runtime.InteropServices;

    internal static class ProcessStartInfo
    {
        public static System.Diagnostics.ProcessStartInfo Create(
            string name, string args, string workingDirectory, bool captureOutput, string windowsName, string windowsArgs, bool windowsPassthrough) =>
            (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                ? new System.Diagnostics.ProcessStartInfo
                {
                    FileName = windowsPassthrough ? windowsName ?? name : "cmd.exe",
                    Arguments = windowsPassthrough ? windowsArgs ?? args : $"/c \"\"{windowsName ?? name}\" {windowsArgs ?? args}\"",
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
