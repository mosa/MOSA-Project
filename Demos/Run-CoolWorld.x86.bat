cd %~dp0
cd ..\bin
start Mosa.Tool.Launcher.exe -autostart -autoexit -autolaunch -output-map -output-asm -output-debug -output-inlined -threading-off -inline-off Mosa.CoolWorld.x86.exe
