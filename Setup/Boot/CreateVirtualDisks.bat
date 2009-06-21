..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe VirtualPC\mosaboot-vhd.config build\bootimage.vhd
..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe Vmware\mosaboot-vhd.config build\bootimage.vhd
..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe VirtualBox\mosaboot-vdi.config build\bootimage.vdi
..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe Qemu\mosaboot-img.config build\bootimage.img
..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe Floppy\mosaboot-floppy.config build\floppy144.img

CALL MakeISOImage.bat
