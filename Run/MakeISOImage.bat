if exist build\iso del /q /f /s build\iso 

if not exist build\iso mkdir build\iso 

del /q /f build\bootimage.iso

copy ..\Tools\syslinux\mboot.c32 build\iso
copy ..\Tools\syslinux\isolinux.bin build\iso
copy iso\syslinux\isolinux.cfg build\iso
copy build\main.exe build\iso

..\bin\Mosa.Tool.MakeIsoImage.exe -label mosa -boot ..\Tools\syslinux\isolinux.bin -boot-info-table -boot-load-size 4 build\bootimage.iso build\iso


