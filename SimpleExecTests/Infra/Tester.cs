namespace SimpleExecTests.Infra
{
    internal static class Tester
    {
        public static string Path =>
#if NETCOREAPP3_1 && DEBUG
            "../../../../SimpleExecTester/bin/Debug/netcoreapp3.1/SimpleExecTester.dll";
#endif
#if NETCOREAPP3_1 && RELEASE
            "../../../../SimpleExecTester/bin/Release/netcoreapp3.1/SimpleExecTester.dll";
#endif
#if NET6_0 && DEBUG
            "../../../../SimpleExecTester/bin/Debug/net6.0/SimpleExecTester.dll";
#endif
#if NET6_0 && RELEASE
            "../../../../SimpleExecTester/bin/Release/net6.0/SimpleExecTester.dll";
#endif
    }
}
