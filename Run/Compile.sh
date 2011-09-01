#!/bin/bash
[ -d build ] || mkdir build
rm -f ./build/hello.exe
mono ../bin/mosacl.exe -a=x86 -f=PE -pe-file-alignment=4096 -b=mb0.7 -sa -o ./build/hello.exe ../bin/$1
