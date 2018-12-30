namespace SimpleExecTests
{
    using System;
    using SimpleExec;
    using SimpleExecTests.Infra;
    using PublicApiGenerator;
    using Xunit;

    public class Api
    {
        [Fact]
        public void IsUnchanged() =>
            AssertFile.Contains(
#if NETCOREAPP2_2
                "../../../api-netcoreapp2_2.txt",
#endif
                ApiGenerator.GeneratePublicApi(typeof(Command).Assembly).Replace(Environment.NewLine, "\r\n"));
    }
}
