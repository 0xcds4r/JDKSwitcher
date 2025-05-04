#!/bin/bash
set -e

APPNAME="jdkswitcher"
VERSION=1.0.0
ARCH=amd64
BUILD_DIR="JDKSwitcher-deb"
PUBLISH_DIR="JDKSwitcher/bin/Release/net8.0/linux-x64/publish"

# 1. Очистка и подготовка структуры
rm -rf "$BUILD_DIR"
mkdir -p "$BUILD_DIR/DEBIAN"
mkdir -p "$BUILD_DIR/opt/$APPNAME"
mkdir -p "$BUILD_DIR/usr/share/applications"

# 2. Control-файл
cat > "$BUILD_DIR/DEBIAN/control" <<EOF
Package: $APPNAME
Version: $VERSION
Section: utils
Priority: optional
Architecture: $ARCH
Depends: libgtk-3-0, libx11-6
Maintainer: 0xcds4r <querror.codeto@gmail.com>
Description: Simple JDK Switcher with GUI for Ubuntu/Linux
 A graphical tool to switch between installed JDKs.
EOF

# 3. Копируем publish
cp -r "$PUBLISH_DIR"/* "$BUILD_DIR/opt/$APPNAME/"
chmod +x "$BUILD_DIR/opt/$APPNAME/JDKSwitcher"

# 4. .desktop-файл
cat > "$BUILD_DIR/usr/share/applications/JDKSwitcher.desktop" <<EOF
[Desktop Entry]
Name=JDK Switcher
Exec=pkexec env DISPLAY=$DISPLAY XAUTHORITY=$XAUTHORITY LD_LIBRARY_PATH=/opt/jdkswitcher /opt/jdkswitcher/JDKSwitcher
Icon=utilities-terminal
Type=Application
Categories=Utility;
EOF

# 5. Сборка deb
fakeroot dpkg-deb --build "$BUILD_DIR"
echo "\nГотово! Файл: $BUILD_DIR.deb" 