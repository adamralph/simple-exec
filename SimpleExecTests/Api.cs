namespace SimpleExecTests
{
    using System;
    using PublicApiGenerator;
    using SimpleExec;
    using SimpleExecTests.Infra;
    using Xunit;

    public class Api
    {
        [Fact]
        public void IsUnchanged() =>
            AssertFile.Contains(
                "../../../api.txt",
                typeof(Command).Assembly.GeneratePublicApi().Replace(Environment.NewLine, "\r\n"));
    }
}
