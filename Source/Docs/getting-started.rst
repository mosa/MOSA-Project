************************
Getting Started (Legacy)
************************

Download
========

The MOSA project is available as a `zip download <https://github.com/mosa/MOSA-Project/archive/master.zip>`__ or via git:

.. code-block:: bash

   git clone https://github.com/mosa/MOSA-Project.git

Prerequisites
=============

You will also need the following prerequisites:

Windows
-------

Install any `Visual Studio <http://www.visualstudio.com>`__ version 2019 or newer. All editions are supported including the fully-featured free `Community Edition <https://www.visualstudio.com/products/visual-studio-community-vs>`__.

Note: The MOSA source code repository includes `Qemu <http://wiki.qemu.org/Main_Page>`__ virtual emulator for Windows.

The `CodeMaid <http://www.codemaid.net>`__ Visual Studio Extension is strongly recommended for MOSA contributors.

Linux
-----

Install `Mono <http://www.mono-project.com>`__ and `Qemu <http://wiki.qemu.org/Main_Page>`__.

The minimum supported version of Mono is 5.16.

If using the APT package manager you can use the following command to quickly set up QEMU and Mono:

.. code-block:: bash

    sudo apt-get -y install mono-devel qemu qemu-system qemu-system-x86

Mac
---

Install `Mono <http://www.mono-project.com>`__ and `Qemu <http://wiki.qemu.org/Main_Page>`__.

Running on Windows
==================

Double click on the ``Compile.bat`` script in the root directory to compile all the tools, sample kernels, and demos.

Next double click on the ``Launcher.bat`` script, which will bring up the :doc:`MOSA Launcher Tool<tool-launcher>` that can:

- Compile the operating system
- Create a virtual disk image, with the compiled binary and boot loader
- Launch a virtual machine instance (using QEMU by default)

By default, the CoolWorld operating system demo is pre-selected. Click the ``Compile and Run`` button to compile and launch the demo.
