#!/bin/bash
DIR="$(cd "$(dirname "$0")" && pwd)/JDKSwitcher/bin/Release/net8.0/linux-x64/publish"
export LD_LIBRARY_PATH="$DIR"
pkexec env DISPLAY=$DISPLAY XAUTHORITY=$XAUTHORITY LD_LIBRARY_PATH=$LD_LIBRARY_PATH "$DIR/JDKSwitcher" 