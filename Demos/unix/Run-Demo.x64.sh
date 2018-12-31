#!/bin/bash

if [ -z $1 ]; then
    echo "No input file specifed"
    exit
fi

if [ ! -f $1 ]; then
    echo "Input file does not exists ($1)"
    exit
fi

absfile=$(realpath $1)

name=$(basename -- "$absfile")
name="${name%.*}"

cd $(dirname $0)/../../bin

mono Mosa.Tool.Compiler.exe \
	-o ${name}.bin \
	-a x64 \
	--format elf64 \
	--mboot v1 \
	--x86-irq-methods \
	--base-address 0x00500000 \
	${absfile} \
	mscorlib.dll \
	Mosa.Plug.Korlib.dll \
	Mosa.Plug.Korlib.x64.dll

if [ $? -ne 0 ]
then
    echo "compilation failed"
	exit
fi

mono --debug Mosa.Tool.CreateBootImage.exe \
	-o ${name}.img \
	--mbr ../Tools/syslinux/3.72/mbr.bin \
	--boot ../Tools/syslinux/3.72/ldlinux.bin \
	--syslinux \
	--volume-label MOSABOOT \
	--blocks 25000 \
	--filesystem fat16 \
	--format img \
	../Tools/syslinux/3.72/ldlinux.sys \
	../Tools/syslinux/3.72/mboot.c32 \
	../Demos/unix/syslinux.cfg \
	${name}.bin,main.exe

if [ $? -ne 0 ]
then
    echo "disk creation failed"
	exit
fi

qemu-system-x86_64 -drive format=raw,file=${name}.img
