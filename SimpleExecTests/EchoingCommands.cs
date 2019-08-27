namespace SimpleExecTests
{
    using System;
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xbehave;
    using Xunit;

    public class EchoingCommands
    {
        [Scenario]
        public void EchoingACommand()
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}", noEcho: false));

            "Then the command is echoed"
                .x(c => Assert.Contains(c.Step.Scenario.DisplayName, Capture.Error.ToString()));
        }

        [Scenario]
        public void SuppressingCommandEcho()
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run with echo suppressed"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}", noEcho: true));

            "Then the command is not echoed"
                .x(c => Assert.DoesNotContain(c.Step.Scenario.DisplayName, Capture.Error.ToString()));
        }

        [Scenario]
        public void EchoingACommandWithASpecificPrefix(string error)
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}", noEcho: false, echoPrefix: $"{c.Step.Scenario.DisplayName} prefix"));

            "Then the command is echoed"
                .x(c => Assert.Contains(c.Step.Scenario.DisplayName, error = Capture.Error.ToString()));

            "And the echo has the specified prefix"
                .x(c => Assert.Contains($"{c.Step.Scenario.DisplayName} prefix:", error));
        }

        [Scenario]
        public void SuppressingCommandEchoWithASpecificPrefix(string error)
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run with echo suppressed"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}", noEcho: true, echoPrefix: $"{c.Step.Scenario.DisplayName} prefix"));

            "Then the command is not echoed"
                .x(c => Assert.DoesNotContain(c.Step.Scenario.DisplayName, error = Capture.Error.ToString()));

            "And the prefix is not echoed"
                .x(c => Assert.DoesNotContain($"{c.Step.Scenario.DisplayName} prefix", error));
        }
    }
}
