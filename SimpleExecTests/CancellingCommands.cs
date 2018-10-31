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
            var watch = Stopwatch.StartNew();
            using (var cancellationTokenSource = new CancellationTokenSource(50))
            {
                await Command.RunAsync("dotnet", $"exec {Tester.Path} sleep", "", false, cancellationTokenSource.Token);
            }
            watch.Stop();
            Assert.True(watch.Elapsed < TimeSpan.FromMilliseconds(1000), $"Command finished outside window in {watch.Elapsed}");
        }

        [Fact]
        public async Task CancellingAReadingCommandStopsCommand()
        {
            var watch = Stopwatch.StartNew();
            using (var cancellationTokenSource = new CancellationTokenSource(50))
            {
                var result = await Command.ReadAsync("dotnet", $"exec {Tester.Path} sleep", "", false, cancellationTokenSource.Token);
            }
            watch.Stop();
            Assert.True(watch.Elapsed < TimeSpan.FromMilliseconds(1000), $"Command finished outside window in {watch.Elapsed}");
        }
    }
}
