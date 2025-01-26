***************
Getting started
***************

Prerequisites
=============

Windows
=======

On Windows, you can either install `Visual Studio <https://visualstudio.microsoft.com/>`__, or use the command line. As
MOSA currently targets .NET 8, you'll need Visual Studio 2022 17.9 or newer.

Linux
=====

On Linux, the following prerequisites are necessary:

* `.NET 8 <https://dotnet.microsoft.com/>`__
* `Git <https://git-scm.com/>`__ (only if you want to clone and build manually)
* `QEMU <https://www.qemu.org/>`__

If you're using Ubuntu 24.04, you can use the following commands to quickly set everything up:

.. code-block:: bash

	# Register Microsoft package repository
	wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
	sudo dpkg -i packages-microsoft-prod.deb

	# Install the .NET Core SDK
	sudo add-apt-repository universe
	sudo apt-get update
	sudo apt-get -y install dotnet-sdk-8.0

	# Intall QEMU and GIT
	sudo apt-get -y install qemu qemu-system qemu-system-x86 git

All Platforms
=============

If you don't want to use any IDE, you can simply use the command line.

.. tip:: This page will assume you're using the command line.

Download
========

NuGet
=====

The easiest way to get started with MOSA is to install the NuGet templates:

.. code-block:: bash

   dotnet new install Mosa.Templates

You can then create a new project based on one of the templates you've just installed:

.. code-block:: bash

   dotnet new mosakrnl -o MyMosaKernel
   cd MyMosaKernel

.. tip:: ``mosakrnl`` is a template for a cross-architecture kernel.

You may now want to build your newly created kernel using the following command:

.. code-block:: bash

	dotnet build
	cd bin

Finally, you can build your kernel using the GUI launcher tool copied to the Tools directory!

.. code-block:: bash

	Tools/Mosa.Tool.Launcher MyMosaKernel.x86.dll

Clone and build manually
========================

The MOSA project is available on GitHub and can be cloned via Git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project
   cd MOSA-Project

In order to build the solution, you can execute the following command:

.. warning:: On Linux, you'd build the **Mosa.Linux.sln** solution instead of **Mosa.sln**. This is because some
	Windows-only tools haven't been yet ported to other platforms.

.. code-block:: bash

	dotnet build Source/Mosa.sln

If successful, it should show a message similar to the one below. Any warnings may be ignored.

.. code-block:: bash

	[...compiler messages...]

	Build succeeded.
	0 Warning(s)
	0 Error(s)

	Time Elapsed 00:00:01.48

Finally, to launch one of the demo applications, simply execute the GUI launcher tool in the bin directory:

.. code-block:: bash

	bin/Mosa.Tool.Launcher

You can then select the demo application of your choice as source, perhaps modify a few options, then build! You can
learn more about this launcher tool :doc:`here<tool-launcher>`.
