@echo Off

echo %~nx0: Building...
dotnet build --configuration Release --nologo || goto :error

echo %~nx0: Testing...
dotnet test --configuration Release --no-build --nologo %1 %2 || goto :error

goto :EOF
:error
exit /b %errorlevel%
