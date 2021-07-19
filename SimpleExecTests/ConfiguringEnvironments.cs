using System;
using System.Threading.Tasks;
using SimpleExec;
using SimpleExecTests.Infra;
using Xunit;

namespace SimpleExecTests
{
    public static class ConfiguringEnvironments
    {
        [Fact]
        public static async Task ConfiguringEnvironment()
        {
            // act
            var result = await Command.ReadAsync("dotnet", $"exec {Tester.Path} environment", configureEnvironment: env => env["foo"] = "bar");

            // assert
            Assert.Contains("foo=bar", result.StandardOutput, StringComparison.Ordinal);
        }
    }
}
