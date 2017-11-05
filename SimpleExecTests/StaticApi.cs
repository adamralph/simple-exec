namespace SimpleExecTests
{
    using System;
    using System.IO;
    using System.Reflection;
    using Humanizer;
    using SimpleExec;
    using Xbehave;
    using Xbehave.Sdk;
    using Xunit;

    public class UseSubdirectoryAttribute : BeforeAfterScenarioAttribute
    {
        private string originalDirectory;

        public override void Before(MethodInfo methodUnderTest)
        {
            this.originalDirectory = Directory.GetCurrentDirectory();
            var binDirectory = new Uri(Path.GetDirectoryName(MethodBase.GetCurrentMethod().DeclaringType.Assembly.CodeBase)).LocalPath;
            var workingDirectory = Path.Combine(binDirectory, "working");
            Directory.CreateDirectory(workingDirectory);
            Directory.SetCurrentDirectory(workingDirectory);
        }

        public override void After(MethodInfo methodUnderTest) => Directory.SetCurrentDirectory(originalDirectory);
    }

    [UseSubdirectory]
    public class StaticApi
    {
        [Scenario]
        public void RunningACommand(string name, Exception exception)
        {
            "Given a command"
                .x(c => WriteCommand(name = CommandName(c)));

            "When I run the command"
                .x(c => exception = Record.Exception(() => Command.Run(name, "hello", "world")));

            "Then the command succeeds"
                .x(() => Assert.Null(exception));
        }

        [Scenario]
        public void RunningACommandInASubdirectory(string dir, string name, Exception exception)
        {
            $"And a subdirectory"
                .x(() => Directory.CreateDirectory(dir = Path.Combine(Directory.GetCurrentDirectory(), "sub")));

            "Given a command which exists both in the working directory and the subdirectory"
                .x(c =>
                {
                    WriteCommand(name = CommandName(c));
                    WriteCommand(Path.Combine(dir, name));
                });

            "When I run the command with the subdirectory as the working directory"
                .x(c => exception = Record.Exception(() => Command.RunIn(dir, name, "hello", "world")));

            "Then the command succeeds"
                .x(() => Assert.Null(exception));
        }

        private static string CommandName(IStepContext stepContext) =>
            stepContext.Step.Scenario.DisplayName.Kebaberize().Replace("(", "").Replace(")", "") + ".cmd";

        private static void WriteCommand(string path)
        {
            var cmd =
@"@echo off
echo dir: %cd%
echo arguments: %*";

            File.WriteAllText(path, cmd);
        }
    }
}
