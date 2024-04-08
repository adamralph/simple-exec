using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class CancellingCommands
{
    [Fact]
    public static void RunningACommand()
    {
        // arrange
        using var cancellationTokenSource = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var cancellationToken = cancellationTokenSource.Token;
        cancellationTokenSource.Cancel();

        // act
        var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path} sleep", cancellationToken: cancellationToken));

        // assert
        Assert.Equal(cancellationToken, Assert.IsType<OperationCanceledException>(exception).CancellationToken);
    }

    [Fact]
    public static async Task RunningACommandAsync()
    {
        // arrange
        using var cancellationTokenSource = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var cancellationToken = cancellationTokenSource.Token;
#if NET8_0_OR_GREATER
        await cancellationTokenSource.CancelAsync();
#else
        cancellationTokenSource.Cancel();
#endif

        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync("dotnet", $"exec {Tester.Path} sleep", cancellationToken: cancellationToken));

        // assert
        Assert.Equal(cancellationToken, Assert.IsType<TaskCanceledException>(exception).CancellationToken);
    }

    [Fact]
    public static async Task ReadingACommandAsync()
    {
        // arrange
        using var cancellationTokenSource = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var cancellationToken = cancellationTokenSource.Token;
#if NET8_0_OR_GREATER
        await cancellationTokenSource.CancelAsync();
#else
        cancellationTokenSource.Cancel();
#endif

        // act
        var exception = await Record.ExceptionAsync(() => Command.ReadAsync("dotnet", $"exec {Tester.Path} sleep", cancellationToken: cancellationToken));

        // assert
        Assert.Equal(cancellationToken, Assert.IsType<TaskCanceledException>(exception).CancellationToken);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public static async Task RunningACommandAsyncWithCreateNoWindow(bool createNoWindow)
    {
        // arrange
        using var cancellationTokenSource = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var cancellationToken = cancellationTokenSource.Token;

        var command = Command.RunAsync(
            "dotnet", $"exec {Tester.Path} sleep", createNoWindow: createNoWindow, cancellationToken: cancellationToken);

        // act
#if NET8_0_OR_GREATER
        await cancellationTokenSource.CancelAsync();
#else
        cancellationTokenSource.Cancel();
#endif

        // assert
        var exception = await Record.ExceptionAsync(() => command);
        Assert.Equal(cancellationToken, Assert.IsType<TaskCanceledException>(exception).CancellationToken);
    }
}
