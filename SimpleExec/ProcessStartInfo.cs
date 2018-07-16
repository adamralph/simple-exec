namespace SimpleExec
{
    internal static class ProcessStartInfo
    {
        public static System.Diagnostics.ProcessStartInfo Create(
            string name, string args, string workingDirectory, bool captureOutput) =>
            new System.Diagnostics.ProcessStartInfo
            {
                FileName = name,
                Arguments = args,
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = captureOutput
            };
    }
}
