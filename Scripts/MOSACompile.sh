#!/bin/bash
[ -d ../bin/build ] || mkdir ../bin/build
rm -f ../bin/build/main.exe
mono ../bin/mosacl.exe -a=x86 -f=elf32 -b=mb0.7 -sa -ssa -o ../bin/build/main.exe ../bin/$1
