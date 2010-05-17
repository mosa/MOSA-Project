#!/bin/bash
mkdir build
rm ./build/hello.exe
mono ../bin/mosacl.exe -a=x86 -f=PE -pe-file-alignment=4096 -b=mb0.7 -sa -o ./build/hello.exe ../bin/Mosa.HelloWorld.exe ../bin/Mosa.Kernel.dll ../bin/mscorlib.dll
