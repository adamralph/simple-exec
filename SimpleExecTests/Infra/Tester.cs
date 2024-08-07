namespace SimpleExecTests.Infra;

internal static class Tester
{
    public static string Path =>
#if NET6_0 && DEBUG
            "../../../../SimpleExecTester/bin/Debug/net6.0/SimpleExecTester.dll";
#endif
#if NET6_0 && RELEASE
            "../../../../SimpleExecTester/bin/Release/net6.0/SimpleExecTester.dll";
#endif
#if NET8_0 && DEBUG
            $"../../../../SimpleExecTester/bin/Debug/net8.0/SimpleExecTester.dll";
#endif
#if NET8_0 && RELEASE
            $"../../../../SimpleExecTester/bin/Release/net8.0/SimpleExecTester.dll";
#endif
}
