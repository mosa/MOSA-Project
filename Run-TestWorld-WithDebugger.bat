cd Source
CALL Compile-Debug.bat
cd ..\Run
CALL Compile.bat Mosa.TestWorld.x86.exe
start ..\bin\Mosa.Tool.Debugger.exe
CALL Run-QEMUWithTCP.bat

