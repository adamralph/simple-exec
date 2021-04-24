using System;
using System.ComponentModel;
using System.Text;
using SimpleExec;
using SimpleExecTests.Infra;
using Xbehave;
using Xunit;

namespace SimpleExecTests
{
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
                .x(() => Assert.Contains("hello world", output, StringComparison.Ordinal));
        }

        [Scenario]
        [Example(false)]
        [Example(true)]
        public void ReadingACommandAsync(bool largeOutput, string output)
        {
            ("When I read a succeeding command" + (largeOutput ? " with large output" : ""))
                .x(async () => output = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world" + (largeOutput ? " large" : "")));

            "Then I see the command output"
                .x(() => Assert.Contains("hello world", output, StringComparison.Ordinal));
        }

        [Scenario]
        [Example(false)]
        [Example(true)]
        public void ReadingAUnicodeCommand(bool largeOutput, string output)
        {
            ("When I read a Unicode command" + (largeOutput ? " with large output" : ""))
                .x(() => output = Command.Read("dotnet", $"exec {Tester.Path} hello world unicode" + (largeOutput ? " large" : ""), encoding: new UnicodeEncoding()));

            "Then I see Unicode chars in the output"
                .x(() => Assert.Contains("Pi (\u03a0)", output, StringComparison.Ordinal));
        }

        [Scenario]
        [Example(false)]
        [Example(true)]
        public void ReadingAUnicodeCommandAsync(bool largeOutput, string output)
        {
            ("When I read a Unicode command" + (largeOutput ? " with large output" : ""))
                .x(async () => output = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world unicode" + (largeOutput ? " large" : ""), encoding: new UnicodeEncoding()));

            "Then I see Unicode chars in the output"
                .x(() => Assert.Contains("Pi (\u03a0)", output, StringComparison.Ordinal));
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

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example(" ")]
        public void ReadingNoCommand(string name, Exception exception)
        {
            "When I read no command"
                .x(() => exception = Record.Exception(
                    () => Command.Read(name)));

            "Then an ArgumentException exception is thrown"
                .x(() => Assert.IsType<ArgumentException>(exception));
        }

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example(" ")]
        public void ReadingNoCommandAsync(string name, Exception exception)
        {
            "When I read no command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.ReadAsync(name)));

            "Then an ArgumentException exception is thrown"
                .x(() => Assert.IsType<ArgumentException>(exception));
        }
    }
}
