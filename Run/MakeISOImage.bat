mkdir build\iso
mkdir build\iso\boot
mkdir build\iso\boot\grub

copy ISO\menu.lst build\iso\boot\grub
copy ..\Tools\grub\stage2_eltorito build\iso\boot\grub
copy build\hello.exe build\iso\boot

..\bin\Mosa.Tool.MakeIsoImage.exe -label mosa -boot ..\Tools\grub\stage2_eltorito -boot-info-table -boot-load-size 4 build\bootimage.iso build\iso

