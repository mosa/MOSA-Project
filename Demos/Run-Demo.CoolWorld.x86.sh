#!/bin/bash
cd ../bin
dotnet Mosa.Tool.Launcher.Console.dll -autostart -qemu -img -output-map -output-asm -output-debug -output-inlined Mosa.Demo.CoolWorld.x86.dll