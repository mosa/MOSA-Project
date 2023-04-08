cd %~dp0
cd ..\bin
start Mosa.Tool.Launcher -autostart -o0 -output-asm -output-debug -output-hash -vmdk -vmware-svga -inline-off -threading-off -inline-explicit-off -include Include Mosa.Demo.SVGAWorld.x86.dll
