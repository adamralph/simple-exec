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
        public static void ReadingACommand(bool largeOutput)
        {
            // act
            var output = Command.Read("dotnet", $"exec {Tester.Path} hello world" + (largeOutput ? " large" : ""));

            // assert
            Assert.Contains("hello world", output, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public static async Task ReadingACommandAsync(bool largeOutput)
        {
            // act
            var output = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world" + (largeOutput ? " large" : ""));

            // assert
            Assert.Contains("hello world", output, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public static void ReadingAUnicodeCommand(bool largeOutput)
        {
            // act
            var output = Command.Read("dotnet", $"exec {Tester.Path} hello world unicode" + (largeOutput ? " large" : ""), encoding: new UnicodeEncoding());

            // assert
            Assert.Contains("Pi (\u03a0)", output, StringComparison.Ordinal);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public static async Task ReadingAUnicodeCommandAsync(bool largeOutput)
        {
            // act
            var output = await Command.ReadAsync("dotnet", $"exec {Tester.Path} hello world unicode" + (largeOutput ? " large" : ""), encoding: new UnicodeEncoding());

            // assert
            Assert.Contains("Pi (\u03a0)", output, StringComparison.Ordinal);
        }

        [Fact]
        public static void ReadingAFailingCommand()
        {
            // act
            var exception = Record.Exception(() => Command.Read("dotnet", $"exec {Tester.Path} error hello world"));

            // assert
            Assert.Equal(1, Assert.IsType<ExitCodeException>(exception).ExitCode);
        }

        [Fact]
        public static async Task ReadingAFailingCommandAsync()
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.ReadAsync("dotnet", $"exec {Tester.Path} error hello world"));

            // assert
            Assert.Equal(1, Assert.IsType<ExitCodeException>(exception).ExitCode);
        }

        [Fact]
        public static void ReadingANonExistentCommand()
        {
            // act
            var exception = Record.Exception(() => Command.Read("simple-exec-tests-non-existent-command"));

            // assert
            _ = Assert.IsType<Win32Exception>(exception);
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
        public static void ReadingNoCommand(string name)
        {
            // act
            var exception = Record.Exception(() => Command.Read(name));

            // assert
            Assert.Equal(nameof(name), Assert.IsType<ArgumentException>(exception).ParamName);
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
