cd Source
CALL Compile-Debug.bat
cd ..\Run
CALL Compile.bat Mosa.CoolWorld.x86.exe
START notepad.exe build\hello.map
START notepad.exe build\asm.txt
CALL Run-PeterBochs-Debugger.bat

