************************
Getting Started On Linux
************************

Prerequisites
=============

The following prerequisites are necessary:

* `.NET 6 <https://dotnet.microsoft.com//>`__
* `Git <https://git-scm.com/>`__
* `QEMU <https://www.qemu.org/>`__

You can also install `Rider <https://www.jetbrains.com/rider/>`__, which is paid, though it does offer a 30-day free trial.

.. tip:: This page will assume you're using the command line.

If using the APT package manager, you can use the following commands to quickly set everything up:

.. code-block:: bash

	# Register Microsoft package repository
	wget -q https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
	sudo dpkg -i packages-microsoft-prod.deb

	# Install the .NET Core SDK
	sudo add-apt-repository universe
	sudo apt-get update
	sudo apt-get install apt-transport-https
	sudo apt-get update
	sudo apt-get install dotnet-sdk-6.0
	sudo snap install dotnet-sdk --classic
	sudo snap alias dotnet-sdk.dotnet dotnet

	# Intall QEMU and GIT
	sudo apt-get -y install qemu qemu-system qemu-system-x86 git

Download
========

The MOSA project is available on GitHub and can be cloned via Git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project

Build
=====

Next, compile the tool set:

.. code-block:: bash

	cd MOSA-Project
	dotnet build Source/Mosa.Linux.sln

If built via the command line, a successful build will display a ``Build succeeded`` message (like below). Any warnings may be ignored.

.. code-block:: bash

	[...compiler messages...]

	Build succeeded.
	0 Warning(s)
	0 Error(s)

	Time Elapsed 00:00:01.48

Launch
======

To launch one of the demo applications, simply start the :doc:`MOSA Launcher Tool<tool-launcher>`. This tool:

- Compiles the operating system 
- Creates a virtual disk image, with the compiled binary and boot loader
- Launches a virtual machine instance (using QEMU by default)

Starter Project
===============

A pre-built starter C# project template is available for experimentation.

You can build it with the commands below:

.. code-block:: bash

	cd MOSA-Project/Source/Mosa.Demo.MyWorld.x86
	dotnet run

