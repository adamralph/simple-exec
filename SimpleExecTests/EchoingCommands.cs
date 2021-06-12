using System;
using System.Runtime.CompilerServices;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests
{
    public static class EchoingCommands
    {
        [Fact]
        public static void EchoingACommand()
        {
            // arrange
            Console.SetError(Capture.Error);

            // act
            Command.Run("dotnet", $"exec {Tester.Path} {TestName()}");

            // assert
            Assert.Contains(TestName(), Capture.Error.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public static void SuppressingCommandEcho()
        {
            // arrange
            Console.SetError(Capture.Error);

            // act
            Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", noEcho: true);

            // assert
            Assert.DoesNotContain(TestName(), Capture.Error.ToString(), StringComparison.Ordinal);
        }

        [Fact]
        public static void EchoingACommandWithASpecificPrefix()
        {
            // arrange
            Console.SetError(Capture.Error);

            // act
            Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", noEcho: false, logPrefix: $"{TestName()} prefix");

            // assert
            var error = Capture.Error.ToString();

            Assert.Contains(TestName(), error, StringComparison.Ordinal);
            Assert.Contains($"{TestName()} prefix:", error, StringComparison.Ordinal);
        }

        [Fact]
        public static void SuppressingCommandEchoWithASpecificPrefix()
        {
            // arrange
            Console.SetError(Capture.Error);

            // act
            Command.Run("dotnet", $"exec {Tester.Path} {TestName()}", noEcho: true, logPrefix: $"{TestName()} prefix");

            // assert
            var error = Capture.Error.ToString();

            Assert.DoesNotContain(TestName(), error, StringComparison.Ordinal);
            Assert.DoesNotContain($"{TestName()} prefix:", error, StringComparison.Ordinal);
        }

        private static string TestName([CallerMemberName] string _ = default) => _;
    }
}
