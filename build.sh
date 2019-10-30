#!/usr/bin/env bash
set -euo pipefail

echo "${0##*/}": Building...
dotnet build --configuration Release --nologo

echo "${0##*/}": Testing...
dotnet test --configuration Release --no-build --nologo
