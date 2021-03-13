using System.Security;
using SimpleExecTests.Infra;
using static SimpleExec.Command;

var password = new SecureString();

password.AppendChar('p');
password.AppendChar('a');
password.AppendChar('s');
password.AppendChar('s');

await RunAsync(
    "dotnet",
    $"exec {Tester.Path}",
    domain: "my-domain",
    loadUserProfile: true
    //password: my-password,
    //passwordInClearText: "my-password-in-clear-text",
    //userName: "my-user-name",
    );
