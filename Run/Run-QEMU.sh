#!/bin/bash
mono ../bin/Mosa.Tool.CreateBootImage.exe IMG/mosaboot-img.config build/bootimage.img
qemu-system-x86_64 -hda build/bootimage.img
