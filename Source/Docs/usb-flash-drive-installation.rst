############################
USB Flash Drive Installation
############################

While most of the development and testing of MOSA is done using virtualization software, MOSA does indeed boot on real hardware too.

Below are the steps for deploying a MOSA disk image to a USB flash drive:

.. warning:: These instructions may vary slightly depending on your installation.

1. Create a MOSA disk image using the MOSA Launcher Tool.

2. Download the `dd <http://www.chrysocome.net/dd>`__ utility for Windows.

3. Copy the ``dd.exe`` executable to the build directory (usually a sub-folder under temp):

.. code-block:: text

  %TEMP%\MOSA  

4. Open a command prompt window and change directory to the build directory.

.. code-block:: text

  cd %TEMP%\MOSA 

5. Connect the USB key you wish to ERASE and install the MOSA image onto.

.. danger:: Data on the USB flash drive will be lost!

6. Determine the device path for the USB flash drive.

Get a list all the block devices on your system by typing the command below. Find the one for the USB flash drive you just connected. Be careful, if you select or mistype the wrong device, you can corrupt your hard drive or other storage devices. Unless you understand these steps completely, do not proceed.

.. code-block:: text

  dd -list

7. Type the following and substitute the of= parameter with the device path found in the previous step.

.. code-block:: text

  dd of=\\?\Device\HarddiskX\PartitionX if=bootimage.img bs=512 –progress

8. Wait until all the blocks are written to the USB key before disconnecting it.

9. Now boot a PC or laptop with the USB flash drive connected!

