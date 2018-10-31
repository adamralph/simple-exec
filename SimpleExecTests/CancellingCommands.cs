namespace SimpleExecTests
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xunit;

    public class CancellingCommands
    {
        [Fact]
        public async Task CancellingARunningCommandStopsCommand()
        {
            using (var cancellationTokenSource = new CancellationTokenSource(50))
            {
                await Command.RunAsync("dotnet", $"exec {Tester.Path} sleep", "", false, cancellationTokenSource.Token);
            }
        }

        [Fact]
        public async Task CancellingAReadingCommandStopsCommand()
        {
            using (var cancellationTokenSource = new CancellationTokenSource(50))
            {
                var result = await Command.ReadAsync("dotnet", $"exec {Tester.Path} sleep", "", false, cancellationTokenSource.Token);
            }
        }
    }
}
