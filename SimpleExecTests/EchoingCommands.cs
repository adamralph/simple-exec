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
    }
}
