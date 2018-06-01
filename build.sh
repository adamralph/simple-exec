#!/usr/bin/env bash
set -euo pipefail

echo Restoring packages...
dotnet restore

echo Testing...
cd SimpleExecTests
dotnet xunit -configuration Release
cd ..
