using System;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests
{
    public static class ConfiguringEnvironments
    {
        [Fact]
        public static void ConfiguringEnvironment()
        {
            // act
            var output = Command.Read("dotnet", $"exec {Tester.Path} environment", configureEnvironment: env => env["foo"] = "bar");

            // assert
            Assert.Contains("foo=bar", output, StringComparison.Ordinal);
        }
    }
}
