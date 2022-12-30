**************************
Getting Started On Windows
**************************

Prerequisites
=============

Install any edition of `Visual Studio <https://visualstudio.microsoft.com/>`__ which supports the current .NET version MOSA is targeting. Currently, MOSA is targeting .NET 6, which requires Visual Studio 2022 or newer.

Alternatively, you can install `Rider <https://www.jetbrains.com/rider/>`__, which is paid, though it does offer a 30-day free trial.

.. tip:: This page will assume you're either using Visual Studio or are using the command line.

Note: The MOSA source code repository bundles many tools, such as the `QEMU <http://wiki.qemu.org/Main_Page>`__ virtual emulator for Windows.

Download
========

The MOSA project is available on GitHub and can be cloned via Git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project

Build
=====

Execute the ``Compiler.bat`` script in the base directory in the root directory to build and compile all the MOSA tools, kernels and demos:

.. code-block:: bash

	Compiler.bat

You can also compile from your IDE, or the command line:

.. code-block:: bash

	dotnet build Source\Mosa.sln

If built via the command line, a successful build will display a ``Build succeeded`` message (like below). Any warnings may be ignored.

.. code-block:: bash

	[...compiler messages...]

	Build succeeded.
	0 Warning(s)
	0 Error(s)

	Time Elapsed 00:00:01.48

Launch
======

To launch one of the demo applications, simply execute the ``Launcher.bat`` script to start the :doc:`MOSA Launcher Tool<tool-launcher>`. This tool:

- Compiles the operating system 
- Creates a virtual disk image, with the compiled binary and boot loader
- Launches a virtual machine instance (using QEMU by default)

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

**Option #3** (Via the command line):

If you want to use the command line, you can do so with the commands below:

.. code-block:: bash

	cd MOSA-Project\Source\Mosa.Demo.MyWorld.x86
	dotnet run

