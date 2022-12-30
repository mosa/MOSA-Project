#!/bin/bash
cd ../bin
# Here we're using VMWare and not QEMU because for some reason the SVGA emulation on QEMU doesn't work
./Mosa.Tool.Launcher.Console -autostart -vmware -vmdk -output-map -output-asm -output-debug -include Include Mosa.Demo.SVGAWorld.x86.dll
