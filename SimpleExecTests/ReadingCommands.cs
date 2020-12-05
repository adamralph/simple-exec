namespace SimpleExecTests
{
    using System;
    using System.ComponentModel;
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

        [Scenario]
        public void ReadingAFailingCommand(Exception exception)
        {
            "When I read a failing command"
                .x(() => exception = Record.Exception(
                    () => Command.Read("dotnet", $"exec {Tester.Path} error hello world")));

            "Then a non-zero exit code exception is thrown"
                .x(() => Assert.IsType<NonZeroExitCodeException>(exception));

            "And the exception contains the exit code"
                .x(() => Assert.Equal(1, ((NonZeroExitCodeException)exception).ExitCode));
        }

        [Scenario]
        public void ReadingAFailingCommandAsync(Exception exception)
        {
            "When I read a failing command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.ReadAsync("dotnet", $"exec {Tester.Path} error hello world")));

            "Then a non-zero exit code exception is thrown"
                .x(() => Assert.IsType<NonZeroExitCodeException>(exception));

            "And the exception contains the exit code"
                .x(() => Assert.Equal(1, ((NonZeroExitCodeException)exception).ExitCode));
        }

        [Scenario]
        public void ReadingANonExistentCommand(Exception exception)
        {
            "When I read a non-existent command"
                .x(() => exception = Record.Exception(
                    () => Command.Read("simple-exec-tests-non-existent-command")));

            "Then a Win32Exception exception, of all things, is thrown"
                .x(() => Assert.IsType<Win32Exception>(exception));
        }

        [Scenario]
        public void ReadingANonExistentCommandAsync(Exception exception)
        {
            "When I read a non-existent command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.ReadAsync("simple-exec-tests-non-existent-command")));

            "Then a Win32Exception exception, of all things, is thrown"
                .x(() => Assert.IsType<Win32Exception>(exception));
        }
    }
}
