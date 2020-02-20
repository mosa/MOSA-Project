**************************
Getting Started On Windows
**************************

Prerequisites
-------------

Install any edition of `Visual Studio <http://www.visualstudio.com>`__ version 2019 or newer, including fully-featured free `Community Edition <https://www.visualstudio.com/products/visual-studio-community-vs>`__ is supported!

Note: The MOSA source code repository includes the `Qemu <http://wiki.qemu.org/Main_Page>`__ virtual emulator for Windows.

Download
========

The MOSA project is available as a `zip download <https://github.com/mosa/MOSA-Project/archive/master.zip>`__ or via git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project.git

If download via zip, unzip the file.

Compiling Tools
===============

Once the download has been unzipped or downloaded via git, execute the ``Compiler.bat`` script in the base directory in the root directory.

This will compile all the tools, kernels and demos.



Next double click on the ``Launcher.bat`` script, which will bring up the :doc:`MOSA Launcher Tool<tool-launcher>` that can:

- Compile the operating system
- Create a virtual disk image, with the compiled binary and boot loader
- Launch a virtual machine instance (using QEMU by default)

By default, the CoolWorld operating system demo is pre-selected. Click the ``Compile and Run`` button to compile and launch the demo.
