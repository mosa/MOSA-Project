cd %~dp0
cd ..\bin
dotnet Mosa.Tool.Launcher.dll -autostart -qemu -o9 -output-asm -output-debug -output-hash Mosa.Demo.HelloWorld.x86.dll
