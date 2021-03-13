using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace SimpleExec
{
    internal static class ProcessStartInfo
    {
        public static System.Diagnostics.ProcessStartInfo Create(
            string name,
            string args,
            string workingDirectory,
            bool captureOutput,
            string windowsName,
            string windowsArgs,
            Action<IDictionary<string, string>> configureEnvironment,
            bool createNoWindow,
            Encoding encoding,
            string domain,
            bool loadUserProfile,
            SecureString password,
            string passwordInClearText,
            string userName)
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsName ?? name : name,
                Arguments = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsArgs ?? args : args,
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = captureOutput,
                CreateNoWindow = createNoWindow,
                StandardOutputEncoding = encoding,
                Domain = domain,
                LoadUserProfile = loadUserProfile,
                Password = password,
                PasswordInClearText = passwordInClearText,
                UserName = userName,
            };

            configureEnvironment?.Invoke(startInfo.Environment);

            return startInfo;
        }
    }
}
