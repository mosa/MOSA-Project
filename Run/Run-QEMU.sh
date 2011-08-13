#!/bin/bash
..\bin\Mosa.Tools.CreateBootImage.exe IMG\mosaboot-img.config build\bootimage.img
CD qemu
qemu -hda build/bootimage.img
