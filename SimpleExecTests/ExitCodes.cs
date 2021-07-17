using System.Threading.Tasks;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests
{
    public static class ExitCodes
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        public static void RunningAComand(int exitCode, bool shouldThrow)
        {
            // arrange
            CommandResult result = null;

            // act
            var exception = Record.Exception(() => result = Command.Run("dotnet", $"exec {Tester.Path} {exitCode}", handleExitCode: code => code == 1));

            // assert
            if (shouldThrow)
            {
                Assert.Equal(exitCode, Assert.IsType<RunException>(exception).ExitCode);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(exitCode, result.ExitCode);
            }
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        public static async Task RunningAComandAsync(int exitCode, bool shouldThrow)
        {
            // arrange
            CommandResult result = null;

            // act
            var exception = await Record.ExceptionAsync(async () => result = await Command.RunAsync("dotnet", $"exec {Tester.Path} {exitCode}", handleExitCode: code => code == 1));

            // assert
            if (shouldThrow)
            {
                Assert.Equal(exitCode, Assert.IsType<RunException>(exception).ExitCode);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(exitCode, result.ExitCode);
            }
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        public static async Task ReadingAComandAsync(int exitCode, bool shouldThrow)
        {
            // arrange
            ReadResult result = null;

            // act
            var exception = await Record.ExceptionAsync(async () => result = await Command.ReadAsync("dotnet", $"exec {Tester.Path} {exitCode}", handleExitCode: code => code == 1));

            // assert
            if (shouldThrow)
            {
                Assert.Equal(exitCode, Assert.IsType<ReadException>(exception).ExitCode);
            }
            else
            {
                Assert.NotNull(result);
                Assert.Equal(exitCode, result.ExitCode);
            }
        }
    }
}
