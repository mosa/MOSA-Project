
Echo "#### Compile the Solution First!!! ####"

if not exist output mkdir output

del /q /f Kernel\hello.exe

cd output

..\..\Mosa\bin\mosacl.exe -a=x86 -f=PE --pe-file-alignment=4096 --map=hello.map -b=mb0.7 -o ..\Kernel\hello.exe ..\..\Mosa\Bin\Mosa.HelloWorld.exe

cd ..

pause
