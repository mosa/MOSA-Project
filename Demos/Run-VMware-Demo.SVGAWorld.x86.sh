#!/bin/bash
cd ../bin
# Here we're using VMWare and not QEMU because for some reason the SVGA emulation on QEMU doesn't work
dotnet Mosa.Tool.Launcher.Console.dll -autostart -vmware -vmdk -output-map -output-asm -output-debug -include Source/Mosa.Demo.SVGAWorld.x86/Include Mosa.Demo.SVGAWorld.x86.dll
