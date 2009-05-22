..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe virtualpc\mosaboot-vhd.config build\bootimage.vhd
..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe vmware\mosaboot-vhd.config build\bootimage.vhd
..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe virtualbox\mosaboot-vdi.config build\bootimage.vdi
..\..\Mosa\Bin\Mosa.Tools.CreateBootImage.exe qemu\mosaboot-img.config build\bootimage.img

CALL MakeISOImage.bat
