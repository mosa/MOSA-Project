
Echo "#### Compile the Solution First!!! ####"

if not exist build mkdir build

del /q /f build\main.exe

cd build

..\..\bin\mosacl.exe -a=avr32 -f=ELF32 --elf-file-alignment=4096 --map=hello.map -sa -o ..\build\main.exe ..\..\bin\%1

cd ..
