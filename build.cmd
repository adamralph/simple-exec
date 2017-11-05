:: options
@echo Off
cd %~dp0
setlocal

echo Restoring packages...
dotnet restore || goto :error

echo Testing...
pushd SimpleExecTests
dotnet xunit || goto :error
popd

:: exit
goto :EOF
:error
exit /b %errorlevel%
