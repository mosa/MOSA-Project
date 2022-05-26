cd %~dp0
cd ..\bin
# Here we're using VMWare and not QEMU because for some reason the SVGA emulation on QEMU doesn't work
dotnet Mosa.Tool.Launcher.dll -autostart -include Source/Mosa.Demo.SVGAWorld.x86/Include -vmware -vmdk -output-map -output-asm -output-debug Mosa.Demo.SVGAWorld.x86.dll

