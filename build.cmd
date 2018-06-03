@echo Off
cd %~dp0

echo %~nx0: Restoring...
dotnet restore || goto :error

echo %~nx0: Building and testing...
dotnet test ./SimpleExecTests/SimpleExecTests.csproj --configuration Release || goto :error

goto :EOF
:error
exit /b %errorlevel%
