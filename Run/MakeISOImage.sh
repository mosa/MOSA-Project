#!/bin/bash
mkdir build/iso
mkdir build/iso/boot
mkdir build/iso/boot/grub

cp ISO/menu.lst build/iso/boot/grub
cp ../Tools/grub/stage2_eltorito build/iso/boot/grub
cp build/hello.exe build/iso/boot

../Bin/Mosa.MakeISOImage.exe -label mosa -boot ../Tools/grub/stage2_eltorito -boot-info-table -boot-load-size 4 build/bootimage.iso build/iso
