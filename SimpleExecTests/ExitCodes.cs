using System.Threading.Tasks;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests;

public static class ExitCodes
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public static void RunningACommand(int exitCode, bool shouldThrow)
    {
        // act
        var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path} {exitCode}", handleExitCode: code => code == 1));

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
        var exception = await Record.ExceptionAsync(() => Command.RunAsync("dotnet", $"exec {Tester.Path} {exitCode}", handleExitCode: code => code == 1));

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
        var exception = await Record.ExceptionAsync(async () => _ = await Command.ReadAsync("dotnet", $"exec {Tester.Path} {exitCode}", handleExitCode: code => code == 1));

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
