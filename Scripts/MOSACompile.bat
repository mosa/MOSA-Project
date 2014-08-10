if not exist build mkdir ..\bin\build

if exist ..\bin\build\main.exe del /q /f ..\bin\build\main.exe 

..\bin\mosacl.exe -a=x86 -f=ELF32 --map=..\bin\build\hello.map -b=mb0.7 -sa -ssa -ssa-optimize -o ..\bin\build\main.exe ..\bin\%1 


