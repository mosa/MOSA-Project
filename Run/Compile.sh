#!/bin/bash
[ -d build ] || mkdir build
rm -f ./build/main.exe
mono ../bin/mosacl.exe -a=x86 -f=PE -pe-file-alignment=4096 -b=mb0.7 -sa -ssa -o ./build/main.exe ../bin/$1
