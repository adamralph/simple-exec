@echo Off
cd %~dp0

echo %~nx0: Restoring...
dotnet restore || goto :error

echo %~nx0: Building and testing...
pushd SimpleExecTests
dotnet xunit -configuration Release || goto :error
popd

goto :EOF
:error
exit /b %errorlevel%
