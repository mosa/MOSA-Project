************************
Getting Started On Linux
************************

Prerequisites
=============

The following prerequisites are necessary:

* `Microsoft .NET Core <https://git-scm.com/>`__
* `Git <https://git-scm.com/>`__
* `Qemu <https://www.qemu.org/>`__

If using the APT package manager, you can use the following commands to quickly set everything up:

.. code-block:: bash

	# Register Microsoft package repository
	wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
	sudo dpkg -i packages-microsoft-prod.deb

	# Install the .NET Core SDK
	sudo add-apt-repository universe
	sudo apt-get update
	sudo apt-get install apt-transport-https
	sudo apt-get update
	sudo apt-get install dotnet-sdk-5.0
	sudo snap install dotnet-sdk --classic
	sudo snap alias dotnet-sdk.dotnet dotnet

	# Intall QEMU and GIT
	sudo apt-get -y install qemu qemu-system qemu-system-x86 git

Download
========

The MOSA project is available as a `zip download <https://github.com/mosa/MOSA-Project/archive/master.zip>`__ or via git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project.git

Build
=====

Next, compile the tool set:

.. code-block:: bash

	cd MOSA-Project
	dotnet build Source/Mosa.Linux.sln

Test
====

To validate everything is working properly, execute the unit tests:

.. code-block:: bash

	dotnet bin/Mosa.Utility.UnitTests.dll -oMax -s Emulator.Display=false
