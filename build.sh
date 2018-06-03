#!/usr/bin/env bash
set -euo pipefail

echo "${0##*/}": Restoring...
dotnet restore

echo "${0##*/}": Building and testing...
dotnet test ./SimpleExecTests/SimpleExecTests.csproj --configuration Release
