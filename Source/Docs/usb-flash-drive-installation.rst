############################
USB Flash Drive Installation
############################

While most of the development and testing of MOSA is done using virtualization software, MOSA does indeed boot on real hardware too.

Below are the steps for deploying a MOSA disk image to a USB flash drive:

.. tip:: This page assumes you're using Windows, even though it's using the **dd** utility.

1. Create a MOSA disk image using the MOSA Launcher Tool.

2. Download the `dd <http://www.chrysocome.net/dd>`__ utility for Windows.

3. Copy the ``dd.exe`` executable to the build directory (usually a sub-folder under temp):

.. code-block:: text

  %TEMP%\MOSA  

4. Open a command prompt window and change directory to the build directory.

.. code-block:: text

  cd %TEMP%\MOSA 

5. Connect the USB flash drive you wish to ERASE and install the MOSA image onto.

.. danger:: Data on the USB flash drive will be lost!

6. Determine the device path for the USB flash drive.

Get a list all the block devices on your system by typing the command below. Find the one for the USB flash drive you just connected. 

.. danger:: Be careful! A mistype or wrong drive selection, may corrupt your hard drive or other storage devices. Do not proceed unless you completely understand these steps.

.. code-block:: text

  dd -list

7. Type the following and substitute the of= parameter with the device path found in the previous step.

.. code-block:: text

  dd of=\\?\Device\HarddiskX\PartitionX if=bootimage.img bs=512 –progress

8. Wait until all the blocks are written to the USB flash drive before disconnecting it.

9. Now boot a PC or laptop with the USB flash drive connected!

