cd %~dp0
cd ..\bin
start Mosa.Tool.Launcher.exe -autostart -autoexit -qemu -o0 -output-asm -inline-off Mosa.Demo.HelloWorld.x86.dll
