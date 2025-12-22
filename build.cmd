@echo Off

echo %~nx0: Formatting...
dotnet format --verify-no-changes || goto :error

echo %~nx0: Building...
dotnet build --configuration Release --nologo || goto :error

echo %~nx0: Testing...
dotnet test --configuration Release --no-build || goto :error

goto :EOF
:error
exit /b %errorlevel%
