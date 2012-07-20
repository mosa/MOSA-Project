..\bin\Mosa.Tool.CreateBootImage.exe VirtualPC\mosaboot-vhd.config build\bootimage.vhd
..\bin\Mosa.Tool.CreateBootImage.exe Vmware\mosaboot-vhd.config build\bootimage.vhd
..\bin\Mosa.Tool.CreateBootImage.exe VirtualBox\mosaboot-vdi.config build\bootimage.vdi
..\bin\Mosa.Tool.CreateBootImage.exe IMG\mosaboot-img.config build\bootimage.img
rem ..\bin\Mosa.Tool.CreateBootImage.exe Floppy\mosaboot-floppy.config build\floppy144.img

copy VirtualPC\MOSA.vmc build\MOSA.vmc
copy VMware\MOSA.vmx build\MOSA.vmx

CALL MakeISOImage.bat
