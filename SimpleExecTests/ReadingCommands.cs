namespace SimpleExecTests
{
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xbehave;
    using Xunit;

    public class ReadingCommands
    {
        [Scenario]
        [Example(false)]
        [Example(true)]
        public void ReadingACommand(bool largeOutput, string output)
        {
            ("When I read a succeeding command" + (largeOutput ? " with large output" : ""))
                .x(() => output = Command.Read("dotnet", $"exec {Tester.Path} hello world" + (largeOutput ? " large" : "")));

            "Then I see the command output"
                .x(() => Assert.Contains("hello world", output));
        }

        [Scenario]
        [Example(false)]
        [Example(true)]
        public void ReadingACommandAsync(bool largeOutput, string output)
        {
            ("When I read a succeeding command" + (largeOutput ? " with large output" : ""))
                .x(async () => output = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world" + (largeOutput ? " large" : "")));

            "Then I see the command output"
                .x(() => Assert.Contains("hello world", output));
        }
    }
}
