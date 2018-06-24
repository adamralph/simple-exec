@echo Off
cd %~dp0

echo %~nx0: Building...
dotnet build --configuration Release || goto :error

echo %~nx0: Testing...
dotnet test ./SimpleExecTests/SimpleExecTests.csproj --configuration Release --no-build || goto :error

goto :EOF
:error
exit /b %errorlevel%
