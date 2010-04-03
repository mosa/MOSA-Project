#!/bin/bash
mono ../bin/Mosa.Tools.CreateBootImage.exe IMG/mosaboot-img.config build/bootimage.img
mono ../bin/Mosa.Tools.CreateBootImage.exe VirtualBox/mosaboot-vdi.config build/bootimage.vdi
