cd %~dp0
cd ..\bin
Mosa.Tool.Launcher.exe -autostart -qemu -output-map -output-asm -output-debug -inline-off -inline-explicit Mosa.BareMetal.HelloWorld.x86.dll
