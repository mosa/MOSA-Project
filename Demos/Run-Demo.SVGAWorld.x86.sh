#!/bin/bash
cd ../bin
# Here we're using VMWare and not QEMU because for some reason the SVGA emulation on QEMU doesn't work
./Mosa.Tool.Launcher.Console -autostart -include Source/Mosa.Demo.SVGAWorld.x86/Include -vmware -output-map -output-asm -output-debug -grub2.00 -iso Mosa.Demo.SVGAWorld.x86.dll