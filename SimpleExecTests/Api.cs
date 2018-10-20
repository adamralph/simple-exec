#if NETCOREAPP2_1
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
                "../../../api-netcoreapp2_1.txt",
                ApiGenerator.GeneratePublicApi(typeof(Command).Assembly).Replace(Environment.NewLine, "\r\n"));
    }
}
#endif
