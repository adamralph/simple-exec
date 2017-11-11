namespace SimpleExecTests
{
    using System;
    using SimpleExec;
    using Xbehave;
    using Xunit;

    public class RunningCommands
    {
        public static readonly string executable =
#if DEBUG
            $"../../../../SimpleExecTester/bin/Debug/netcoreapp2.0/SimpleExecTester.dll";
#else
            $"../../../../SimpleExecTester/bin/Release/netcoreapp2.0/SimpleExecTester.dll";
#endif

        [Scenario]
        public void RunningASucceedingCommand(Exception exception)
        {
            "When I run a succeeding command"
                .x(() => exception = Record.Exception(
                    () => Command.Run("dotnet", $"exec {executable} hello world")));

            "Then no exception is thrown"
                .x(() => Assert.Null(exception));
        }

        [Scenario]
        public void RunningAFailingCommand(Exception exception)
        {
            "When I run a failing command"
                .x(() => exception = Record.Exception(
                    () => Command.Run("dotnet", $"exec {executable} error hello world")));

            "Then an exception is thrown"
                .x(() => Assert.NotNull(exception));

            "And the exception message contains the exit code"
                .x(() => Assert.Contains("code 1", exception.Message));

            "And the exception message contains the contents of stderr"
                .x(() => Assert.Contains($"error hello world", exception.Message));
        }

        [Scenario]
        public void RunningASucceedingCommandAsync(Exception exception)
        {
            "When I run a succeeding command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.RunAsync("dotnet", $"exec {executable} hello world")));

            "Then no exception is thrown"
                .x(() => Assert.Null(exception));
        }

        [Scenario]
        public void RunningAFailingCommandAsync(Exception exception)
        {
            "When I run a failing command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.RunAsync("dotnet", $"exec {executable} error hello world")));

            "Then an exception is thrown"
                .x(() => Assert.NotNull(exception));

            "And the exception message contains the exit code"
                .x(() => Assert.Contains("code 1", exception.Message));

            "And the exception message contains the contents of stderr"
                .x(() => Assert.Contains($"error hello world", exception.Message));
        }
    }
}
