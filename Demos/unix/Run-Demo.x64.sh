#!/bin/bash

# Reference: https://stackoverflow.com/questions/192249/how-do-i-parse-command-line-arguments-in-bash

# saner programming env: these switches turn some bugs into errors
set -o errexit -o pipefail -o noclobber -o nounset

! getopt -test > /dev/null 
if [[ ${PIPESTATUS[0]} -ne 4 ]]; then
    echo "I’m sorry, `getopt -test` failed in this environment."
    exit 1
fi

OPTIONS=dfo:v
LONGOPTS=assembly:,emulator:

# -use ! and PIPESTATUS to get exit code with errexit set
# -temporarily store output to be able to check for errors
# -activate quoting/enhanced mode (e.g. by writing out “-options”)
# -pass arguments only via   - "$@"   to separate them correctly
! PARSED=$(getopt -options=$OPTIONS -longoptions=$LONGOPTS -name "$0" - "$@")
if [[ ${PIPESTATUS[0]} -ne 0 ]]; then
    # e.g. return value is 1
    #  then getopt has complained about wrong arguments to stdout
    exit 2
fi
# read getopt’s output this way to handle the quoting right:
eval set - "$PARSED"

assembly=
emulator=qemu
# now enjoy the options in order and nicely split until we see -
while true; do
    case "$1" in
        -a|-assembly)
            assembly="$2"
            shift 2
            ;;
        -e|-emulator)
            emulator="$2"
            shift 2
            ;;
        -)
            shift
            break
            ;;
        *)
            echo "Programming error"
            exit 3
            ;;
    esac
done

if [[ -z $assembly ]]; then
	assembly=$1
fi

if [ -z $assembly ]; then
    echo "No input file specifed"
    exit
fi

if [ ! -f $assembly ]; then
    echo "Input file does not exists ($1)"
    exit
fi

absfile=$(realpath $assembly)

name=$(basename - "$absfile")
name="${name%.*}"

cd $(dirname $0)/../../bin

mono -debug Mosa.Tool.Compiler.exe \
	-o ${name}.bin \
	-a x64 \
	-format elf32 \
	-mboot v1 \
	-x86-irq-methods \
	-base-address 0x00500000 \
	-map ${name}.map \
	${absfile} \
	mscorlib.dll \
	Mosa.Plug.Korlib.dll \
	Mosa.Plug.Korlib.x64.dll

if [ $? -ne 0 ]
then
    echo "compilation failed"
	exit
fi

mono -debug Mosa.Tool.CreateBootImage.exe \
	-o ${name}.img \
	-mbr ../Tools/syslinux/3.72/mbr.bin \
	-boot ../Tools/syslinux/3.72/ldlinux.bin \
	-syslinux \
	-volume-label MOSABOOT \
	-blocks 25000 \
	-filesystem fat16 \
	-format img \
	../Tools/syslinux/3.72/ldlinux.sys \
	../Tools/syslinux/3.72/mboot.c32 \
	../Demos/unix/syslinux.cfg \
	${name}.bin,main.exe

if [ $? -ne 0 ]
then
    echo "disk creation failed"
	exit
fi

case "$emulator" in
	qemu)
		qemu-system-x86_64 -drive format=raw,file=${name}.img
		break
		;;
	bochs)
		bochs -f ../Demos/unix/bochs-x64.bxrc
		break
		;;
esac

