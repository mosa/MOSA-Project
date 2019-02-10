#!/bin/sh

# This file will be invoked from the gdb script. For internal use only.

qemu-system-i386 -kernel bin/Mosa.HelloWorld.x86.bin -S -gdb stdio
