#!/usr/bin/env bash
set -euo pipefail

echo "${0##*/}": Restoring...
dotnet restore

echo "${0##*/}": Building and testing...
cd SimpleExecTests
dotnet xunit -configuration Release
cd ..
