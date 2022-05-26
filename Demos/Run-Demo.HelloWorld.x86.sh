#!/bin/bash
cd ../bin
dotnet Mosa.Tool.Launcher.Console.dll -autostart -qemu -o9 -output-asm -output-debug Mosa.Demo.HelloWorld.x86.dll