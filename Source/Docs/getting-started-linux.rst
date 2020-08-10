************************
Getting Started On Linux
************************

Prerequisites
=============

You will also need the following prerequisites:

# `Qemu <https://www.qemu.org/>`__.
# `Git <https://git-scm.com/>`__.
# `Microsoft .NET Core <https://git-scm.com/>`__.
# `Mono <http://www.mono-project.com>`__

If using the APT package manager you can use the following commands to quickly set everything up:

.. code-block:: bash

	# Register Microsoft package repository
	wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
	sudo dpkg -i packages-microsoft-prod.deb

	# Install the .NET Core SDK
	sudo add-apt-repository universe
	sudo apt-get update
	sudo apt-get install apt-transport-https
	sudo apt-get update
	sudo apt-get install dotnet-sdk-3.1
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

.. code-block:: bash

	cd MOSA-Project
	dotnet restore Source/Mosa.Linux.sln
	dotnet msbuild /verbosity:minimal Source/Mosa.Linux.sln
