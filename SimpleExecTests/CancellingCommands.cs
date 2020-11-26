namespace SimpleExecTests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xbehave;
    using Xunit;

    public class CancellingCommands
    {
        [Scenario]
        public void RunningACommandAsync(CancellationTokenSource cancellationTokenSource, Task command, Exception exception)
        {
            "Given a cancellation token source"
                .x(c => cancellationTokenSource = new CancellationTokenSource().Using(c));

            "When I start running a long-running command using the cancellation token"
                .x(() =>
                {
                    command = Command.RunAsync("dotnet", $"exec {Tester.Path} sleep", cancellationToken: cancellationTokenSource.Token);
                });

            "And I cancel the cancellation token source"
                .x(() => cancellationTokenSource.Cancel());

            "And I await the command"
                .x(async () => exception = await Record.ExceptionAsync(async () => await command));

            "Then an OperationCanceledException is Thrown"
                .x(() => Assert.IsAssignableFrom<OperationCanceledException>(exception));
        }

        [Scenario]
        public void ReadingACommandAsync(CancellationTokenSource cancellationTokenSource, Task command, Exception exception)
        {
            "Given a cancellation token source"
                .x(c => cancellationTokenSource = new CancellationTokenSource().Using(c));

            "When I start reading a long-running command using the cancellation token"
                .x(() =>
                {
                    command = Command.ReadAsync("dotnet", $"exec {Tester.Path} sleep", cancellationToken: cancellationTokenSource.Token);
                });

            "And I cancel the cancellation token source"
                .x(() => cancellationTokenSource.Cancel());

            "And I await the command"
                .x(async () => exception = await Record.ExceptionAsync(async () => await command));

            "Then an OperationCanceledException is Thrown"
                .x(() => Assert.IsAssignableFrom<OperationCanceledException>(exception));
        }

        [Scenario]
        public void RunningACommand(CancellationTokenSource cancellationTokenSource, Task command, Exception exception)
        {
            "Given a cancellation token source"
                .x(c => cancellationTokenSource = new CancellationTokenSource().Using(c));

            "When I start running a long-running command using the cancellation token"
                .x(() =>
                {
                    command = Task.Run(() => Command.Run("dotnet", $"exec {Tester.Path} sleep", cancellationToken: cancellationTokenSource.Token));
                });

            "And I cancel the cancellation token source"
                .x(() => cancellationTokenSource.Cancel());

            "And I await the command"
                .x(async () => exception = await Record.ExceptionAsync(async () => await command));

            "Then an OperationCanceledException is Thrown"
                .x(() => Assert.IsAssignableFrom<OperationCanceledException>(exception));
        }

        [Scenario]
        public void ReadingACommand(CancellationTokenSource cancellationTokenSource, Task command, Exception exception)
        {
            "Given a cancellation token source"
                .x(c => cancellationTokenSource = new CancellationTokenSource().Using(c));

            "When I start reading a long-running command using the cancellation token"
                .x(() =>
                {
                    command = Task.Run(() => Command.Read("dotnet", $"exec {Tester.Path} sleep", cancellationToken: cancellationTokenSource.Token));
                });

            "And I cancel the cancellation token source"
                .x(() => cancellationTokenSource.Cancel());

            "And I await the command"
                .x(async () => exception = await Record.ExceptionAsync(async () => await command));

            "Then an OperationCanceledException is Thrown"
                .x(() => Assert.IsAssignableFrom<OperationCanceledException>(exception));
        }
    }
}
