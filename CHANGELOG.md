# Changelog

## 7.0.0

### Enhancements

- [#254: **[BREAKING]** throw ArgumentException when command name missing](https://github.com/adamralph/simple-exec/pull/254)
- [#279: **[BREAKING]** Support preferred encoding when reading commands](https://github.com/adamralph/simple-exec/pull/279)

### Fixed bugs

- [#253: **[BREAKING]** Reading a non-existent command throws an InvalidOperationException](https://github.com/adamralph/simple-exec/pull/253)
- [#281: Missing ConfigureAwait(false) in ReadAsync](https://github.com/adamralph/simple-exec/pull/281)

## 6.4.0

### Enhancements

- [#230: Pass CancellationToken To RunAsync and ReadAsync](https://github.com/adamralph/simple-exec/issues/230)
- [#249: add remark about Read deadlocks](https://github.com/adamralph/simple-exec/pull/249)

## 6.3.0

### Enhancements

- [#183: upgrade to SourceLink 1.0.0](https://github.com/adamralph/simple-exec/pull/183)
- [#222: Add support for the CreateNoWindow option](https://github.com/adamralph/simple-exec/issues/222)

## 6.2.0

### Enhancements

- [#174: Support passing environment variables to processes](https://github.com/adamralph/simple-exec/issues/174)

## 6.1.0

### Enhancements

- [#137: Update SourceLink to 1.0.0-beta2-19367-01](https://github.com/adamralph/simple-exec/issues/137)
- [#140: Prefix messages in stderr](https://github.com/adamralph/simple-exec/issues/140)
- [#143: Add XML documentation file to package 🤦‍♂](https://github.com/adamralph/simple-exec/issues/143)

## 6.0.0

### Enhancements

- [#128: **[BREAKING]** replace cmd.exe usage with optional params for windows](https://github.com/adamralph/simple-exec/pull/128)

## 5.0.1

### Fixed bugs

- [#112: The filename, directory name, or volume label syntax is incorrect.](https://github.com/adamralph/simple-exec/issues/112)

## 5.0.0

### Enhancements

- [#98: upgrade Source Link to 1.0.0-beta2-18618-05](https://github.com/adamralph/simple-exec/pull/98)
- [#100: **[BREAKING]** Use cmd.exe on Windows](https://github.com/adamralph/simple-exec/issues/100)
- [#111: Handle large output when reading](https://github.com/adamralph/simple-exec/pull/111)

### Other

- [#104: **[BREAKING]** Remove deprecated CommandException](https://github.com/adamralph/simple-exec/issues/104)

## 4.2.0

### Enhancements

- [#84: Throw a NonZeroExitCodeException, with an ExitCode property](https://github.com/adamralph/simple-exec/issues/84)

## 4.1.0

### Enhancements

- [#81: Throw CommandException instead of Exception](https://github.com/adamralph/simple-exec/issues/81)

## 4.0.0

### Enhancements

- [#57: Add API documentation](https://github.com/adamralph/simple-exec/issues/57)
- [#63: **[BREAKING]** Switch to optional parameters](https://github.com/adamralph/simple-exec/issues/63)
- [#64: **[BREAKING]** Echo to stderr instead of stdout](https://github.com/adamralph/simple-exec/issues/64)

## 3.0.0

### Enhancements

- [#48: **[BREAKING]** Don't redirect stderr](https://github.com/adamralph/simple-exec/issues/48)

## 2.3.0

### Enhancements

- [#36: Echo suppression](https://github.com/adamralph/simple-exec/pull/36)
- [#44: Source stepping](https://github.com/adamralph/simple-exec/pull/44)

### Fixed bugs

- [#45: ConfigureAwait(false) is missing in a few places](https://github.com/adamralph/simple-exec/pull/45)

## 2.2.0

### Enhancements

- [#31: add .NET Standard 2.0 target](https://github.com/adamralph/simple-exec/pull/31)

## 2.1.0

### Enhancements

- [#18: Read() method, capturing stdout](https://github.com/adamralph/simple-exec/issues/18)

## 2.0.0

### Enhancements

- [#4: build using release config](https://github.com/adamralph/simple-exec/pull/4)
- [#5: capture stderr in exception message](https://github.com/adamralph/simple-exec/pull/5)
- [#7: Async API](https://github.com/adamralph/simple-exec/issues/7)
- [#9: **[BREAKING]** Simpler API](https://github.com/adamralph/simple-exec/issues/9)
- [#10: Echo the command in the console (stdout)](https://github.com/adamralph/simple-exec/issues/10)

## 1.0.0

### Enhancements

- [#1: Run commands with args and optional working directory](https://github.com/adamralph/simple-exec/issues/1)
