#!/bin/bash
mono ../bin/Mosa.Tool.CreateBootImage.exe IMG/mosaboot-img.config build/bootimage.img
qemu -hda build/bootimage.img
