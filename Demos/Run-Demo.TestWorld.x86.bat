cd %~dp0
cd ..\bin
dotnet Mosa.Tool.Launcher.dll -autostart -output-map -output-asm -inline-off -output-debug -output-hash Mosa.Demo.TestWorld.x86.dll

