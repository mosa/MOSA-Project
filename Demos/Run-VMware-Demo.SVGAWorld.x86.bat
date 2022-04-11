cd %~dp0
cd ..\bin
dotnet Mosa.Tool.Launcher.dll -autostart -include Source/Mosa.Demo.SVGAWorld.x86/Include -vmware -vmdk -output-map -output-asm -output-debug Mosa.Demo.SVGAWorld.x86.dll