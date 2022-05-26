cd %~dp0
cd ..\bin
dotnet Mosa.Tool.Launcher.dll -autostart -qemu -output-map -output-asm -output-debug -inline-off -inline-explicit Mosa.BareMetal.HelloWorld.x86.dll
