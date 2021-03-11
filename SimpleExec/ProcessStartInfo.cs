using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SimpleExec
{
    internal static class ProcessStartInfo
    {
        public static System.Diagnostics.ProcessStartInfo Create(
            string name, string args, string workingDirectory, bool captureOutput, string windowsName, string windowsArgs, Action<IDictionary<string, string>> configureEnvironment, bool createNoWindow, Encoding encoding)
        {
            var startInfo = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new System.Diagnostics.ProcessStartInfo
                {
                    FileName = windowsName ?? name,
                    Arguments = windowsArgs ?? args,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = captureOutput,
                    CreateNoWindow = createNoWindow,
                    StandardOutputEncoding = encoding,
                }
                : new System.Diagnostics.ProcessStartInfo
                {
                    FileName = name,
                    Arguments = args,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = captureOutput,
                    CreateNoWindow = createNoWindow,
                    StandardOutputEncoding = encoding,
                };

            configureEnvironment?.Invoke(startInfo.Environment);

            return startInfo;
        }
    }
}
