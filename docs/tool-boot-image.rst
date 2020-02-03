####################
MOSA Boot Image Tool
####################

The Boot Image Tool is a command line application that can be used to create bootable images containing a MOSA operating system application. 

.. warning:: This tool has been superceded by the MOSA Launcher Tool which can automate the entire build chain functionality.

The tool several command line options. Sample:

.. code-block:: text

  Mosa.Tool.CreateBootImage.exe -o bin/Mosa.HelloWorld.x86.img --mbr Tools/syslinux/3.72/mbr.bin --boot Tools/syslinux/3.72/ldlinux.bin --syslinux --volume-label MOSABOOT --blocks 25000 --filesystem fat16 --format img Tools/syslinux/3.72/ldlinux.sys Tools/syslinux/3.72/mboot.c32 Demos/unix/syslinux.cfg bin/Mosa.HelloWorld.x86.bin,main.exe

The following options are supported:

.. list-table:: Arguments
  :header-rows: 1

  * - Option
    - Arguments
    - Description
  * - --volume
    - Volume Name
    - Set the volume name for the first partition
  * - --blocks
    - # of Blocks
    - Set the number of 512-byte blocks
  * - --filesystem
    - fat12/fat16/fat32
    - File System type
  * - --format
    - img/vhd/vdi/vmdk/img
    - Disk Image Format
  * - --syslinux
    -
    - Patch disk image for syslinux
  * - --mbr
    - Filename
    - Use file for Master Boot Record
  * - --boot
    - Filename
    - Use file for Boot Record
  * - 
    - Filename[,Destination]
    - Include file in file system. Optional Destination will rename the file

     
The tool can create disk images for the following emulators:

.. csv-table:: File formats
  :header: "Emulator", "File format"

  Virtual PC 2004/2007, .VHD
  Virtual Server, .VHD
  VMware, .VHD
  VirtualBox, .VDI
  QEMU, .IMG
  Raw Image, .IMG
