cd Source
CALL Compile-Debug.bat
cd ..\Run
CALL Compile.bat Mosa.HelloWorld.x86.exe
start ..\bin\Mosa.Tool.Debugger.exe
CALL CreateVirtualDisks.bat
start build\MOSA.vmc
