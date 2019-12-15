namespace SimpleExec
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal static class ProcessStartInfo
    {
        public static System.Diagnostics.ProcessStartInfo Create(
            string name, string args, string workingDirectory, bool captureOutput, string windowsName, string windowsArgs, Action<IDictionary<string, string>> configureEnvironment)
        {
            var startInfo = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new System.Diagnostics.ProcessStartInfo
                {
                    FileName = windowsName ?? name,
                    Arguments = windowsArgs ?? args,
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

            configureEnvironment?.Invoke(startInfo.Environment);

            return startInfo;
        }
    }
}
