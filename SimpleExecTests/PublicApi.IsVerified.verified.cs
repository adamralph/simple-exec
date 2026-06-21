namespace SimpleExec
{
    public static class Command
    {
        [return: System.Runtime.CompilerServices.TupleElementNames(new string[] {
                "StandardOutput",
                "StandardError"})]
        public static Task<ValueTuple<string, string>> ReadAsync(
                    string name,
                    IEnumerable<string> args,
                    string workingDirectory = "",
                    Action<IDictionary<string, string?>>? configureEnvironment = null,
                    Func<int, bool>? handleExitCode = null,
                    System.Text.Encoding? encoding = null,
                    string? standardInput = null,
                    bool cancellationIgnoresProcessTree = false,
                    CancellationToken ct = default) { }
        [return: System.Runtime.CompilerServices.TupleElementNames(new string[] {
                "StandardOutput",
                "StandardError"})]
        public static Task<ValueTuple<string, string>> ReadAsync(
                    string name,
                    string args = "",
                    string workingDirectory = "",
                    Action<IDictionary<string, string?>>? configureEnvironment = null,
                    Func<int, bool>? handleExitCode = null,
                    System.Text.Encoding? encoding = null,
                    string? standardInput = null,
                    bool cancellationIgnoresProcessTree = false,
                    CancellationToken ct = default) { }
        public static void Run(
                    string name,
                    IEnumerable<string> args,
                    string workingDirectory = "",
                    Action<IDictionary<string, string?>>? configureEnvironment = null,
                    IEnumerable<string>? secrets = null,
                    Func<int, bool>? handleExitCode = null,
                    string? echoPrefix = null,
                    bool noEcho = false,
                    bool cancellationIgnoresProcessTree = false,
                    bool createNoWindow = false,
                    CancellationToken ct = default) { }
        public static void Run(
                    string name,
                    string args = "",
                    string workingDirectory = "",
                    Action<IDictionary<string, string?>>? configureEnvironment = null,
                    IEnumerable<string>? secrets = null,
                    Func<int, bool>? handleExitCode = null,
                    string? echoPrefix = null,
                    bool noEcho = false,
                    bool cancellationIgnoresProcessTree = false,
                    bool createNoWindow = false,
                    CancellationToken ct = default) { }
        public static Task RunAsync(
                    string name,
                    IEnumerable<string> args,
                    string workingDirectory = "",
                    Action<IDictionary<string, string?>>? configureEnvironment = null,
                    IEnumerable<string>? secrets = null,
                    Func<int, bool>? handleExitCode = null,
                    string? echoPrefix = null,
                    bool noEcho = false,
                    bool cancellationIgnoresProcessTree = false,
                    bool createNoWindow = false,
                    CancellationToken ct = default) { }
        public static Task RunAsync(
                    string name,
                    string args = "",
                    string workingDirectory = "",
                    Action<IDictionary<string, string?>>? configureEnvironment = null,
                    IEnumerable<string>? secrets = null,
                    Func<int, bool>? handleExitCode = null,
                    string? echoPrefix = null,
                    bool noEcho = false,
                    bool cancellationIgnoresProcessTree = false,
                    bool createNoWindow = false,
                    CancellationToken ct = default) { }
    }
    public class ExitCodeException : Exception
    {
        public ExitCodeException(
                    int exitCode) { }
        public ExitCodeException(
                    int exitCode,
                    Exception innerException) { }
        public ExitCodeException(
                    int exitCode,
                    string message) { }
        public ExitCodeException(
                    int exitCode,
                    string message,
                    Exception innerException) { }
        public int ExitCode { get; }
        protected static string CreateMessage(
                    int exitCode) { }
    }
    public class ExitCodeReadException : ExitCodeException
    {
        public ExitCodeReadException(
                    int exitCode,
                    string standardOutput,
                    string standardError) { }
        public ExitCodeReadException(
                    int exitCode,
                    string standardOutput,
                    string standardError,
                    Exception innerException) { }
        public ExitCodeReadException(
                    int exitCode,
                    string standardOutput,
                    string standardError,
                    string message) { }
        public ExitCodeReadException(
                    int exitCode,
                    string standardOutput,
                    string standardError,
                    string message,
                    Exception innerException) { }
        public string StandardError { get; }
        public string StandardOutput { get; }
        protected static string CreateMessage(
                    int exitCode,
                    string standardOutput,
                    string standardError) { }
    }
}
