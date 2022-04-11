#!/bin/bash
cd ../bin
dotnet Mosa.Tool.Launcher.Console.dll -autostart -output-map -output-asm -inline-off -output-debug Mosa.Demo.TestWorld.x86.dll