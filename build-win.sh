#!/bin/bash
set -e

APPNAME="JDKSwitcher"
PUBLISH_DIR="JDKSwitcher/bin/Release/net8.0/win-x64/publish"

# 1. Сборка под Windows x64, self-contained, с .exe
dotnet publish JDKSwitcher -c Release -r win-x64 --self-contained true

# 2. Сообщение о результате
echo "Сборка завершена!"
echo "Файл: $PUBLISH_DIR/$APPNAME.exe"
ls -lh "$PUBLISH_DIR/$APPNAME.exe"