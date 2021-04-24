using System;
using SimpleExec;
using SimpleExecTests.Infra;
using Xbehave;
using Xunit;

namespace SimpleExecTests
{
    public class EchoingCommands
    {
        [Scenario]
        public void EchoingACommand()
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}"));

            "Then the command is echoed"
                .x(c => Assert.Contains(c.Step.Scenario.DisplayName, Capture.Error.ToString(), StringComparison.Ordinal));
        }

        [Scenario]
        public void SuppressingCommandEcho()
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run with echo suppressed"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}", noEcho: true));

            "Then the command is not echoed"
                .x(c => Assert.DoesNotContain(c.Step.Scenario.DisplayName, Capture.Error.ToString(), StringComparison.OrdinalIgnoreCase));
        }

        [Scenario]
        public void EchoingACommandWithASpecificPrefix(string error)
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}", noEcho: false, echoPrefix: $"{c.Step.Scenario.DisplayName} prefix"));

            "Then the command is echoed"
                .x(c => Assert.Contains(c.Step.Scenario.DisplayName, error = Capture.Error.ToString(), StringComparison.Ordinal));

            "And the echo has the specified prefix"
                .x(c => Assert.Contains($"{c.Step.Scenario.DisplayName} prefix:", error, StringComparison.Ordinal));
        }

        [Scenario]
        public void SuppressingCommandEchoWithASpecificPrefix(string error)
        {
            "Given console output is being captured"
                .x(() => Console.SetError(Capture.Error));

            "When a command is run with echo suppressed"
                .x(c => Command.Run("dotnet", $"exec {Tester.Path} {c.Step.Scenario.DisplayName}", noEcho: true, echoPrefix: $"{c.Step.Scenario.DisplayName} prefix"));

            "Then the command is not echoed"
                .x(c => Assert.DoesNotContain(c.Step.Scenario.DisplayName, error = Capture.Error.ToString(), StringComparison.OrdinalIgnoreCase));

            "And the prefix is not echoed"
                .x(c => Assert.DoesNotContain($"{c.Step.Scenario.DisplayName} prefix", error, StringComparison.OrdinalIgnoreCase));
        }
    }
}
