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
        using var cts = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var ct = cts.Token;
        cts.Cancel();

        // act
        var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path} sleep", ct: ct));

        // assert
        Assert.Equal(ct, Assert.IsType<OperationCanceledException>(exception).CancellationToken);
    }

    [Fact]
    public static async Task RunningACommandAsync()
    {
        // arrange
        using var cts = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var ct = cts.Token;
        await cts.CancelAsync();

        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync("dotnet", $"exec {Tester.Path} sleep", ct: ct));

        // assert
        Assert.Equal(ct, Assert.IsType<TaskCanceledException>(exception).CancellationToken);
    }

    [Fact]
    public static async Task ReadingACommandAsync()
    {
        // arrange
        using var cts = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var ct = cts.Token;
        await cts.CancelAsync();

        // act
        var exception = await Record.ExceptionAsync(() => Command.ReadAsync("dotnet", $"exec {Tester.Path} sleep", ct: ct));

        // assert
        Assert.Equal(ct, Assert.IsType<TaskCanceledException>(exception).CancellationToken);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public static async Task RunningACommandAsyncWithCreateNoWindow(bool createNoWindow)
    {
        // arrange
        using var cts = new CancellationTokenSource();

        // use a cancellation token source to ensure value type equality comparison in assertion is meaningful
        var ct = cts.Token;

        var command = Command.RunAsync(
            "dotnet", $"exec {Tester.Path} sleep", createNoWindow: createNoWindow, ct: ct);

        // act
        await cts.CancelAsync();

        // assert
        var exception = await Record.ExceptionAsync(() => command);
        Assert.Equal(ct, Assert.IsType<TaskCanceledException>(exception).CancellationToken);
    }
}
