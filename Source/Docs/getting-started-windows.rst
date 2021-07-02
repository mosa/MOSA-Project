**************************
Getting Started On Windows
**************************

Prerequisites
=============

Install any edition of `Visual Studio <http://www.visualstudio.com>`__ version 2019 or newer.

Note: The MOSA source code repository includes the `Qemu <http://wiki.qemu.org/Main_Page>`__ virtual emulator for Windows.

Download
========

The MOSA project is available as a `zip download <https://github.com/mosa/MOSA-Project/archive/master.zip>`__ or via git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project.git

If download via zip, unzip the file.

Build
=====

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

To launch one of the demo application, execute the ``Launcher.bat`` script to start the :doc:`MOSA Launcher Tool<tool-launcher>`. This tool:

- Compiles the operating system 
- Creates a virtual disk image, with the compiled binary and boot loader
- Launches a virtual machine instance (using QEMU by default)

By default, the CoolWorld demo operating system is pre-selected. Click the ``Compile and Run`` button to compile and launch the demo.

Starter Project
===============

A pre-built starter C# project template is available for experimentation.

**Option #1** (Preferred - includes the entire MOSA project):

Open the ``Source\Mosa.sln`` solution with Visual Studio and locate the ``Mosa.Demo.MyWorld.x86`` under ``Demos``.

Next, set the project as ``Set as Startup Project``.

To compile and launch the application within a virtual machine, select from the ``Debug`` menu the ``Start Without Debugging`` option, or press ``CTRL+F5``.

**Option #2** (Only the starter project):

Open the ``Source\Mosa.Demo.MyWorld.x86\Mosa.Demo.MyWorld.x86.csproj`` project with Visual Studio.

To compile and launch the application within a virtual machine, select from the ``Debug`` menu the ``Start Without Debugging`` option, or press ``CTRL+F5``.

