namespace SimpleExecTests.Infra
{
    internal static class Tester
    {
        public static string Path =>
#if NETCOREAPP2_1 && DEBUG
            $"../../../../SimpleExecTester/bin/Debug/netcoreapp2.1/SimpleExecTester.dll";
#endif
#if NETCOREAPP2_1 && RELEASE
            $"../../../../SimpleExecTester/bin/Release/netcoreapp2.1/SimpleExecTester.dll";
#endif
#if NETCOREAPP3_1 && DEBUG
            $"../../../../SimpleExecTester/bin/Debug/netcoreapp3.1/SimpleExecTester.dll";
#endif
#if NETCOREAPP3_1 && RELEASE
            $"../../../../SimpleExecTester/bin/Release/netcoreapp3.1/SimpleExecTester.dll";
#endif
#if NET5_0 && DEBUG
            $"../../../../SimpleExecTester/bin/Debug/net5.0/SimpleExecTester.dll";
#endif
#if NET5_0 && RELEASE
            $"../../../../SimpleExecTester/bin/Release/net5.0/SimpleExecTester.dll";
#endif
    }
}
