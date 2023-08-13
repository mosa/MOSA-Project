# curl https://qemu.weilnetz.de/w64/qemu-w64-setup-20230810.exe --output qemu-installer.exe
# del /S /Q qemu
# 7zip\7z.exe x -wqemu -oqemu -x!*.nsis -y qemu-installer.exe
# del /Q qemu-installer.exe

del /S /Q qemu\share\icons
del /S /Q qemu\share\doc
del /S /Q qemu\share\man

del /S /Q qemu\qemu-edid.exe
del /S /Q qemu\qemu-ga.exe
del /S /Q qemu\qemu-io.exe
del /S /Q qemu\qemu-storage-daemon.exe
del /S /Q qemu\qemu-system-aarch64w.exe
del /S /Q qemu\qemu-system-alpha.exe
del /S /Q qemu\qemu-system-alphaw.exe
del /S /Q qemu\qemu-system-avr.exe
del /S /Q qemu\qemu-system-avrw.exe
del /S /Q qemu\qemu-system-cris.exe
del /S /Q qemu\qemu-system-crisw.exe
del /S /Q qemu\qemu-system-hppa.exe
del /S /Q qemu\qemu-system-hppaw.exe
del /S /Q qemu\qemu-system-loongarch64.exe
del /S /Q qemu\qemu-system-loongarch64w.exe
del /S /Q qemu\qemu-system-m68k.exe
del /S /Q qemu\qemu-system-m68kw.exe
del /S /Q qemu\qemu-system-microblaze.exe
del /S /Q qemu\qemu-system-microblazeel.exe
del /S /Q qemu\qemu-system-microblazeelw.exe
del /S /Q qemu\qemu-system-microblazew.exe
del /S /Q qemu\qemu-system-mips.exe
del /S /Q qemu\qemu-system-mips64.exe
del /S /Q qemu\qemu-system-mips64el.exe
del /S /Q qemu\qemu-system-mips64elw.exe
del /S /Q qemu\qemu-system-mips64w.exe
del /S /Q qemu\qemu-system-mipsel.exe
del /S /Q qemu\qemu-system-mipselw.exe
del /S /Q qemu\qemu-system-mipsw.exe
del /S /Q qemu\qemu-system-nios2.exe
del /S /Q qemu\qemu-system-nios2w.exe
del /S /Q qemu\qemu-system-or1k.exe
del /S /Q qemu\qemu-system-or1kw.exe
del /S /Q qemu\qemu-system-ppc.exe
del /S /Q qemu\qemu-system-ppc64.exe
del /S /Q qemu\qemu-system-ppc64w.exe
del /S /Q qemu\qemu-system-ppcw.exe
del /S /Q qemu\qemu-system-riscv32.exe
del /S /Q qemu\qemu-system-riscv32w.exe
del /S /Q qemu\qemu-system-riscv64.exe
del /S /Q qemu\qemu-system-riscv64w.exe
del /S /Q qemu\qemu-system-rx.exe
del /S /Q qemu\qemu-system-rxw.exe
del /S /Q qemu\qemu-system-s390x.exe
del /S /Q qemu\qemu-system-s390xw.exe
del /S /Q qemu\qemu-system-sh4.exe
del /S /Q qemu\qemu-system-sh4eb.exe
del /S /Q qemu\qemu-system-sh4ebw.exe
del /S /Q qemu\qemu-system-sh4w.exe
del /S /Q qemu\qemu-system-sparc.exe
del /S /Q qemu\qemu-system-sparc64.exe
del /S /Q qemu\qemu-system-sparc64w.exe
del /S /Q qemu\qemu-system-sparcw.exe
del /S /Q qemu\qemu-system-tricore.exe
del /S /Q qemu\qemu-system-tricorew.exe
del /S /Q qemu\qemu-system-xtensa.exe
del /S /Q qemu\qemu-system-xtensaeb.exe
del /S /Q qemu\qemu-system-xtensaebw.exe
del /S /Q qemu\qemu-system-xtensaw.exe
del /S /Q qemu\qemu-system-i386w.exe
del /S /Q qemu\qemu-system-armw.exe
del /S /Q qemu\qemu-system-x86_64w.exe
del /S /Q qemu\$PLUGINSDIR

del /S /Q qemu\lib\gdk-pixbuf-2.0\2.10.0\qemu-uninstall.exe.nsis
del /S /Q qemu\share\petalogix-*.dtb
del /S /Q qemu\share\trace-events-all
del /S /Q qemu\share\s390-ccw.img


