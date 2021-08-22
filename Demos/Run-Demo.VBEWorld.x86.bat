cd %~dp0
cd ..\bin
start Mosa.Tool.Launcher.exe -autostart -autoexit -qemu -output-map -output-asm -output-debug -grub2.00 -iso -video -video-width 640 -video-height 480 -video-depth 32 Mosa.Demo.VBEWorld.x86.dll

