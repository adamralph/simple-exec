namespace SimpleExecTests
{
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xbehave;
    using Xunit;

    public class ConfiguringEnvironments
    {
        [Scenario]
        public void ConfiguringEnvironment(string output)
        {
            "When I read a command which echos the 'foo' environment variable and value"
                .x(() => output = Command.Read(
                    "dotnet",
                    $"exec {Tester.Path} environment",
                    configureEnvironment: env => env["foo"] = "bar"));

            "Then I see the echoed 'foo' environment variable and value"
                .x(() => Assert.Contains("foo=bar", output));
        }
    }
}
