cd %~dp0
cd ..\bin
start Mosa.Tool.Launcher.exe --autostart --autoexit --qemu --output-map --output-asm --output-debug Mosa.BareMetal.HelloWorld.x86.exe
