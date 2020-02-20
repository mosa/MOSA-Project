**************************
Getting Started On Windows
**************************

Prerequisites
=============

Install any edition of `Visual Studio <http://www.visualstudio.com>`__ version 2019 or newer, including fully-featured free `Community Edition <https://www.visualstudio.com/products/visual-studio-community-vs>`__ is supported!

Note: The MOSA source code repository includes the `Qemu <http://wiki.qemu.org/Main_Page>`__ virtual emulator for Windows.

Download
========

The MOSA project is available as a `zip download <https://github.com/mosa/MOSA-Project/archive/master.zip>`__ or via git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project.git

If download via zip, unzip the file.

Compile the Tools
===================

Execute the ``Compiler.bat`` script in the base directory in the root directory to build and compile all the MOSA tools, kernels and demos.

.. code-block:: bash

	Compiler.bat

A successful build will display a ``Build succedded`` message (like below). Warnings may be ignored.

.. code-block:: bash

	[... lines removed...]

	Build succeeded.

	[... lines removed...]

    1 Warning(s)
    0 Error(s)

	Time Elapsed 00:00:01.48

Launch
======

Execute the ``Launcher.bat`` script to start the :doc:`MOSA Launcher Tool<tool-launcher>`. This tool:

- Compiles the operating system 
- Creates a virtual disk image, with the compiled binary and boot loader
- Launches a virtual machine instance (using QEMU by default)

By default, the CoolWorld demo operating system is pre-selected. Click the ``Compile and Run`` button to compile and launch the demo.
