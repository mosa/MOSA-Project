cd %~dp0
cd ..\bin
dotnet Mosa.Tool.Launcher.dll -autostart -output-map -output-asm -output-debug -output-inlined Mosa.Demo.CoolWorld.x86.dll
