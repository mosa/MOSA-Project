
Echo "#### Compile the Solution First!!! ####"

if not exist build mkdir build

del /q /f build\hello.exe

cd build

..\..\bin\mosacl.exe -a=x86 -f=PE --pe-file-alignment=4096 --map=hello.map -b=mb0.7 -o ..\build\hello.exe ..\..\bin\Mosa.HelloWorld.exe -sa

cd ..

