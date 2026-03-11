#!/bin/bash
# Run this script from: CareBridge.Api/
# Usage: bash rename-to-carebridge.sh

set -e
cd "$(dirname "$0")/.."
if [ -d "DotNet Starter Template" ] && [ ! -d "CareBridge" ]; then
  mv "DotNet Starter Template" "CareBridge"
  echo "Done. Folder renamed to CareBridge."
else
  echo "Folder already renamed or not found. Current dirs:"
  ls -1
fi
