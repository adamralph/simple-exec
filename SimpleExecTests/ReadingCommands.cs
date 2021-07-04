using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests
{
    public static class ReadingCommands
    {
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public static async Task ReadingACommandAsync(bool largeOutput)
        {
            // act
            var result = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world" + (largeOutput ? " large" : ""));

            // assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("hello world", result.Out, StringComparison.Ordinal);
            Assert.Contains("hello world", result.Error, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public static async Task ReadingAUnicodeCommandAsync(bool largeOutput)
        {
            // act
            var result = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world unicode" + (largeOutput ? " large" : ""), encoding: new UnicodeEncoding());

            // assert
            Assert.Equal(0, result.ExitCode);
            Assert.Contains("Pi (\u03a0)", result.Out, StringComparison.Ordinal);
            Assert.Contains("Pi (\u03a0)", result.Error, StringComparison.Ordinal);
        }

        [Fact]
        public static async Task ReadingAFailingCommandAsync()
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.ReadAsync("dotnet", $"exec {Tester.Path} 1 hello world"));

            // assert
            var exitCodeReadException = Assert.IsType<ExitCodeReadException>(exception);
            Assert.Equal(1, exitCodeReadException.ExitCode);
            Assert.Contains("hello world", exitCodeReadException.Out, StringComparison.Ordinal);
            Assert.Contains("hello world", exitCodeReadException.Error, StringComparison.Ordinal);
        }

        [Fact]
        public static async Task ReadingANonExistentCommandAsync()
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.ReadAsync("simple-exec-tests-non-existent-command"));

            // assert
            _ = Assert.IsType<Win32Exception>(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public static async Task ReadingNoCommandAsync(string name)
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.ReadAsync(name));

            // assert
            Assert.Equal(nameof(name), Assert.IsType<ArgumentException>(exception).ParamName);
        }
    }
}
