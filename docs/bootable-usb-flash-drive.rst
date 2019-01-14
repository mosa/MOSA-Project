########################################
How to Create a Bootable USB Flash Drive 
########################################

.. warning:: These instructions maybe out of date.

While most of the development and testing of MOSA is done using virtualization software, MOSA does indeed boot on real hardware too. 

Here are detailed instructions for deploying MOSA operating system to a USB flash drive:

1. Download the `dd <http://www.chrysocome.net/dd>`__ utility for Windows.

2. Copy the ``dd.exe`` executable to the build directory (usually a sub-folder under temp):

.. code-block:: text

  %TEMP%\MOSA  

3. Open a command prompt window and change directory to the build directory.

.. code-block:: text

  cd %TEMP%\MOSA 

4. Connect the USB key you wish to ERASE and install the MOSA image onto.

.. danger:: Data on the USB flash drive will be lost!

5. Determine the device path for the USB flash drive.

Get a list all the block devices on your system by typing the command below. Find the one for the USB flash drive you just connected. Be careful, if you select or mistype the wrong device, you can corrupt your hard drive or other storage devices. Unless you understand these steps completely, do not proceed.

.. code-block:: text

  dd -list

6. Type the following and substitute the of= parameter with the device path found in the previous step.

.. code-block:: text

  dd of=\\?\Device\HarddiskX\PartitionX if=bootimage.img bs=512 –progress

7. Wait until all the blocks are written to the USB key before disconnecting it.

8. Now boot a PC or laptop with the USB flash drive connected!

