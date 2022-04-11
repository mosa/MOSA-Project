#!/bin/bash
cd ../bin
dotnet Mosa.Tool.Launcher.Console.dll -autostart -qemu -output-map -output-asm -output-debug -inline-off -inline-explicit Mosa.BareMetal.HelloWorld.x86.dll