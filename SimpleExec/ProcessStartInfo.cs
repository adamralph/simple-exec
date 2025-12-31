using System.Text;

namespace SimpleExec;

internal static class ProcessStartInfo
{
    public static System.Diagnostics.ProcessStartInfo Create(
        string name,
        string args,
        IEnumerable<string> argList,
        string workingDirectory,
        Action<IDictionary<string, string?>> configureEnvironment,
        Encoding? encoding,
        bool createNoWindow,
        bool redirectStandardStreams)
    {
        var startInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = name,
            Arguments = args,
            WorkingDirectory = workingDirectory,
            StandardErrorEncoding = encoding,
            StandardOutputEncoding = encoding,
            CreateNoWindow = createNoWindow,
            RedirectStandardError = redirectStandardStreams,
            RedirectStandardInput = redirectStandardStreams,
            RedirectStandardOutput = redirectStandardStreams,
            UseShellExecute = false,
        };

        foreach (var arg in argList)
        {
            startInfo.ArgumentList.Add(arg);
        }

        configureEnvironment(startInfo.Environment);

        return startInfo;
    }
}
