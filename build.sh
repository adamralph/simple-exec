#!/usr/bin/env bash
set -e
set -o pipefail
set -x

echo Restoring packages...
dotnet restore

echo Testing...
cd SimpleExecTests
dotnet xunit -configuration Release
cd ..
