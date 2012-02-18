#!/bin/bash
mono ../bin/Mosa.Tool.CreateBootImage.exe IMG/mosaboot-img.config build/bootimage.img
mono ../bin/Mosa.Tool.CreateBootImage.exe VirtualBox/mosaboot-vdi.config build/bootimage.vdi
