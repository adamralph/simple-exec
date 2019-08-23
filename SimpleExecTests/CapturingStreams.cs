namespace SimpleExecTests
{
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xbehave;
    using Xunit;

    public class CapturingStreams
    {
        [Scenario]
        [Example(false)]
        [Example(true)]
        public void CapturingAStream(bool largeOutput, string output, string error)
        {
            ("When I run a succeeding command" + (largeOutput ? " with large output" : ""))
                .x(() => Command.Run("dotnet", $"exec {Tester.Path} hello world diagnostic" + (largeOutput ? " large" : ""), outputDataReceived: s => output += s, errorDataReceived: s => error += s));

            "Then I capture the command output"
                .x(() => Assert.Contains("hello world", output));

            "And I capture the command error"
                .x(() => Assert.Contains("hello world", error));
        }

        [Scenario]
        [Example(false)]
        [Example(true)]
        public void CapturingAStreamAsync(bool largeOutput, string output, string error)
        {
            ("When I run a succeeding command" + (largeOutput ? " with large output" : ""))
                .x(async () => await Command.RunAsync("dotnet", $"exec {Tester.Path} hello world diagnostic" + (largeOutput ? " large" : ""), outputDataReceived: s => output += s, errorDataReceived: s => error += s));

            "Then I capture the command output"
                .x(() => Assert.Contains("hello world", output));

            "And I capture the command error"
                .x(() => Assert.Contains("hello world", error));
        }
    }
}
