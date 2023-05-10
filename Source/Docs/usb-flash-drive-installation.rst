############################
USB Flash Drive Installation
############################

While most of the development and testing of MOSA is done using virtualization software, MOSA does indeed boot on real hardware too.

There are several ways to put MOSA on a USB flash drive. Below are the most common ones, including the one we'll be following here:

- `Ventoy <https://ventoy.net/>`__ (All platforms)
- `Rufus <https://rufus.ie/>`__ (Windows only)
- dd (Linux, `unofficial Windows version <http://www.chrysocome.net/dd>`__ exists)

We'll be following the ``dd`` way here.

Windows
=======

1. Create a MOSA disk image using the MOSA Launcher Tool.

2. Download the unofficial dd utility for Windows, as linked above.

3. Open a command prompt window and change directory to the build directory (usually a subfolder under ``%TEMP%``).

.. code-block:: text

  cd %TEMP%\MOSA 

4. Copy the ``dd.exe`` executable to the current directory.

5. Connect the USB flash drive you wish to ERASE and install the MOSA image onto.

.. danger:: Data on the USB flash drive will be lost!

6. Determine the device path for the USB flash drive.

Get a list all the block devices on your system by typing the command below. Find the one for the USB flash drive you just connected.

.. danger:: Be careful! A mistype or wrong drive selection may corrupt your hard drive or other storage devices. Do not proceed unless you entirely understand these steps.

.. code-block:: text

  dd -list

7. Type the following and substitute the ``of=`` parameter with the device path found in the previous step.

.. code-block:: text

  dd of=\\?\Device\HarddiskX\PartitionX if=bootimage.img bs=512 –progress

8. Wait until all the blocks are written to the USB flash drive before disconnecting it.

9. Now boot a PC with the USB flash drive connected!

Linux
=====

1. Create a MOSA disk image using the MOSA Launcher Tool.

2. Download the unofficial dd utility for Windows, as linked above.

3. Open a command prompt window and change directory to the build directory (usually a subfolder under ``%TEMP%``).

.. code-block:: text

  cd /tmp/MOSA

4. Connect the USB flash drive you wish to ERASE and install the MOSA image onto.

.. danger:: Data on the USB flash drive will be lost!

5. Determine the device path for the USB flash drive.

Get a list all the block devices on your system by typing the command below. Find the one for the USB flash drive you just connected. 

.. danger:: Be careful! A mistype or wrong drive selection may corrupt your hard drive or other storage devices. Do not proceed unless you entirely understand these steps.

.. code-block:: text

  sudo fdisk -l

6. Type the following and substitute the ``of=`` parameter with the device path found in the previous step.

.. code-block:: text

  dd of=/dev/XXXX if=bootimage.img bs=512 status=progress

7. Wait until all the blocks are written to the USB flash drive before disconnecting it.

8. Now boot a PC with the USB flash drive connected!
