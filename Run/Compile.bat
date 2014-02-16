
Echo "#### Compile the Solution First!!! ####"

if not exist build mkdir build

if exist build\main.exe del /q /f build\main.exe 

cd build

..\..\bin\mosacl.exe -a=x86 -f=PE --map=hello.map -b=mb0.7 -sa -ssa -o ..\build\main.exe ..\..\bin\%1 ..\..\bin\Mosa.Internal.dll

rem -mped=..\..\bin\output 

cd ..

CALL Disassemble.bat

