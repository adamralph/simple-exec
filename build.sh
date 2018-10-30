#!/usr/bin/env bash
set -euo pipefail

echo "${0##*/}": Building...
dotnet build --configuration Release

echo "${0##*/}": Testing...
dotnet test ./SimpleExecTests/SimpleExecTests.csproj --configuration Release --no-build --framework netcoreapp2.1
