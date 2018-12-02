namespace SimpleExecTests
{
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xbehave;
    using Xunit;

    public class ReadingCommands
    {
        [Scenario]
        public void ReadingACommand(string output)
        {
            "When I read a succeeding command"
                .x(() => output = Command.Read("dotnet", $"exec {Tester.Path} hello world"));

            "Then I see the command output"
                .x(() => Assert.Contains("hello world", output));
        }

        [Scenario]
        public void ReadingACommandAsync(string output)
        {
            "When I read a succeeding command"
                .x(async () => output = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world"));

            "Then I see the command output"
                .x(() => Assert.Contains("hello world", output));
        }
    }
}
