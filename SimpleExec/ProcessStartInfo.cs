using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleExec;

internal static class ProcessStartInfo
{
    public static System.Diagnostics.ProcessStartInfo Create(
        string name,
        string args,
        IEnumerable<string> argList,
        string workingDirectory,
        bool redirectStandardStreams,
        Action<IDictionary<string, string?>> configureEnvironment,
        bool createNoWindow,
        Encoding? encoding = null)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = name,
            Arguments = args,
            WorkingDirectory = workingDirectory,
            UseShellExecute = false,
            RedirectStandardError = redirectStandardStreams,
            RedirectStandardInput = redirectStandardStreams,
            RedirectStandardOutput = redirectStandardStreams,
            CreateNoWindow = createNoWindow,
            StandardErrorEncoding = encoding,
            StandardOutputEncoding = encoding,
        };

        foreach (var arg in argList)
        {
            startInfo.ArgumentList.Add(arg);
        }

        configureEnvironment(startInfo.Environment);

        return startInfo;
    }
}
