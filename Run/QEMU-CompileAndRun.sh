#!/bin/bash
./CompileHelloWorld.sh
./CreateVirtualDisks.sh
qemu -hda build/bootimage.img
