
rem #### Compile the solution first ####

if not exist output mkdir output

cd output

..\..\Mosa\bin\mosacl.exe -a=x86 -f=PE --pe-file-alignment=4096 --map=hello.map -b=mb0.7 -o ..\..\Mosa\Bin\Mosa.HelloWorld.exe Mosa.HelloWorld.exe

cd ..

del /q /f Kernel\hello.exe

copy output\hello.exe Kernel\hello.exe

pause
