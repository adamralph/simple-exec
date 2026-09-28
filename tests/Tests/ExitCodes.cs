using SimpleExec;
using Tests.Infra;
using Xunit;

namespace Tests;

public static class ExitCodes
{
    private static Ct Ct => TestContext.Current.CancellationToken;

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public static void RunningACommand(int exitCode, bool shouldThrow)
    {
        // act
        var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Cli.Path} {exitCode}", handleExitCode: code => code == 1, ct: Ct));

        // assert
        if (shouldThrow)
        {
            Assert.Equal(exitCode, Assert.IsType<ExitCodeException>(exception).ExitCode);
        }
        else
        {
            Assert.Null(exception);
        }
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public static async Task RunningACommandAsync(int exitCode, bool shouldThrow)
    {
        // act
        var exception = await Record.ExceptionAsync(() => Command.RunAsync("dotnet", $"exec {Cli.Path} {exitCode}", handleExitCode: code => code == 1, ct: Ct));

        // assert
        if (shouldThrow)
        {
            Assert.Equal(exitCode, Assert.IsType<ExitCodeException>(exception).ExitCode);
        }
        else
        {
            Assert.Null(exception);
        }
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public static async Task ReadingACommandAsync(int exitCode, bool shouldThrow)
    {
        // act
        var exception = await Record.ExceptionAsync(async () => _ = await Command.ReadAsync("dotnet", $"exec {Cli.Path} {exitCode}", handleExitCode: code => code == 1, ct: Ct));

        // assert
        if (shouldThrow)
        {
            Assert.Equal(exitCode, Assert.IsType<ExitCodeReadException>(exception).ExitCode);
        }
        else
        {
            Assert.Null(exception);
        }
    }
}
