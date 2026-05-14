using System.Diagnostics;
using SimpleExec;

//
// Console.WriteLine("Running command...");
// Command.Run(name);
//
// Console.WriteLine("Running command async...");
// await Command.RunAsync(name);
//
// Console.WriteLine("Reading command...");
// var (stdOut, stdErr) = await Command.ReadAsync(name);
//
// Console.WriteLine(stdOut);
//
//
// var psi = new ProcessStartInfo(@"C:\Users\adam\Downloads\openapi-changes.exe")
// {
//     // RedirectStandardOutput = true,
//     // RedirectStandardError = true,
//     UseShellExecute = false,
// };
//
// using var process = new Process { StartInfo = psi };
// process.Start();
//
// // var stdoutTask = process.StandardOutput.ReadToEndAsync();
//
// process.WaitForExit();
//
// // string stdout = await stdoutTask;
//
// // Console.WriteLine(stdout);

const string name = @"C:\Users\adam\Downloads\openapi-changes_0.2.1_windows_x86_64\openapi-changes.exe";
// const string name = @"C:\Users\adam\Downloads\openapi-changes_0.2.6_windows_x86_64\openapi-changes.exe";

using var process = new Process();
process.StartInfo.FileName = name;
process.StartInfo.UseShellExecute = false;
process.StartInfo.RedirectStandardInput = true;
process.StartInfo.CreateNoWindow = true;
process.Start();
process.WaitForExit();
