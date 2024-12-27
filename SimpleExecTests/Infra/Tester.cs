namespace SimpleExecTests.Infra;

internal static class Tester
{
    public static string Path =>
#if NET8_0 && DEBUG
            $"../../../../SimpleExecTester/bin/Debug/net8.0/SimpleExecTester.dll";
#endif
#if NET8_0 && RELEASE
            $"../../../../SimpleExecTester/bin/Release/net8.0/SimpleExecTester.dll";
#endif
#if NET9_0 && DEBUG
            "../../../../SimpleExecTester/bin/Debug/net9.0/SimpleExecTester.dll";
#endif
#if NET9_0 && RELEASE
            "../../../../SimpleExecTester/bin/Release/net9.0/SimpleExecTester.dll";
#endif
}
