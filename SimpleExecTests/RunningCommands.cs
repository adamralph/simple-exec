using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests
{
    public static class RunningCommands
    {
        private static readonly string command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "hello-world.cmd" : "ls";

        [Fact]
        public static void RunningASucceedingCommand()
        {
            // act
            var exception = Record.Exception(() => Command.Run(command));

            // assert
            Assert.Null(exception);
        }

        [Fact]
        public static void RunningASucceedingCommandWithArgs()
        {
            // act
            var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path} hello world"));

            // assert
            Assert.Null(exception);
        }

        [Fact]
        public static async Task RunningASucceedingCommandAsync()
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.RunAsync(command));

            // assert
            Assert.Null(exception);
        }

        [Fact]
        public static void RunningAFailingCommand()
        {
            // act
            var exception = Record.Exception(() => Command.Run("dotnet", $"exec {Tester.Path} error hello world"));

            // assert
            Assert.Equal(1, Assert.IsType<ExitCodeException>(exception).ExitCode);
        }

        [Fact]
        public static async Task RunningAFailingCommandAsync()
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.RunAsync("dotnet", $"exec {Tester.Path} error hello world"));

            // assert
            Assert.Equal(1, Assert.IsType<ExitCodeException>(exception).ExitCode);
        }

        [Fact]
        public static void RunningANonExistentCommand()
        {
            // act
            var exception = Record.Exception(() => Command.Run("simple-exec-tests-non-existent-command"));

            // assert
            _ = Assert.IsType<Win32Exception>(exception);
        }

        [Fact]
        public static async Task RunningANonExistentCommandAsync()
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.RunAsync("simple-exec-tests-non-existent-command"));

            // assert
            _ = Assert.IsType<Win32Exception>(exception);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public static void RunningNoCommand(string name)
        {
            // act
            var exception = Record.Exception(() => Command.Run(name));

            // assert
            Assert.Equal(nameof(name), Assert.IsType<ArgumentException>(exception).ParamName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public static async Task RunningNoCommandAsync(string name)
        {
            // act
            var exception = await Record.ExceptionAsync(() => Command.RunAsync(name));

            // assert
            Assert.Equal(nameof(name), Assert.IsType<ArgumentException>(exception).ParamName);
        }
    }
}
