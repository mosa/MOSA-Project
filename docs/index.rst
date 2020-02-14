############
Introduction
############

MOSA is an open source software project that natively executes .NET applications within a virtual hypervisor or on bare metal hardware!

The MOSA project consists of:

- Compiler - a high quality, multithreaded, cross-platform, optimizing .NET compiler
- Kernel - a small kernel operating system
- Device Drivers Framework - a modular, device drivers framework and device drivers
- Debugger - QEMU-based debugger

**************
Current Status
**************

The target platforms are:

- Intel X86/32-bit (stable)
- Intel X64 (in development)
- ARM v6 (in early development)

The MOSA compiler supports nearly all and object oriented non-object oriented code, including:

- Generic Code (example: List<T>)
- Delegates (static and non-static) and with optional parameters
- Exception Handling (try, finally, and catch code blocks)

The MOSA compiler seeks to provide high quality code generation using the following optimizations:

- Constant Folding and Propagation 
- Strength Reduction optimization
- Dead Code Elimination
- Single Static Assignment (SSA)
- Global Value Numbering / Common Subexpession Elimination
- Sparse Conditional Constant Propagation
- Inlined Expansion
- Loop-Invariant Code Motion
- Block Reordering
- Greedy Register Allocation

***************
Getting Started
***************

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

Install any `Visual Studio <http://www.visualstudio.com>`__ version 2018 or newer. All editions are supported including the fully-featured free `Community Edition <https://www.visualstudio.com/products/visual-studio-community-vs>`__.

Note: The MOSA source code repository includes `Qemu <http://wiki.qemu.org/Main_Page>`__ virtual emulator for Windows.

The `CodeMaid <http://www.codemaid.net>`__ Visual Studio Extension is strongly recommended for MOSA contributors.

Linux
-----

Install `Mono <http://www.mono-project.com>`__ and `Qemu <http://wiki.qemu.org/Main_Page>`__.

The minimum supported version of Mono is 5.16.

If using the APT package manager you can use the following command to quickly set up QEMU and Mono

.. code-block:: bash

    sudo apt-get -y install mono-devel qemu

Mac
---

Install `Mono <http://www.mono-project.com>`__ and `Qemu <http://wiki.qemu.org/Main_Page>`__.

Running on Windows
==================

Double click on the "Compile.bat" script in the root directory to compile all the tools, sample kernels, and demos.

Next double click on the "Launcher.bat" script, which will bring up the MOSA Launcher tool (screenshot below) that can:

- Compile the operating system
- Create a virtual disk image, with the compiled binary and boot loader
- Launch a virtual machine instance (QEMU by default)

By default, the CoolWorld operating system demo is pre-selected. Click the "Compile and Run" button to compile and launch the demo.

*******************
Join the Discussion
*******************

Join us on `Gitter chat <https://gitter.im/mosa/MOSA-Project>`__. This is the most interactive way to connect to MOSA's development team.

*******
License
*******

MOSA is licensed under the `New BSD License <http://en.wikipedia.org/wiki/New_BSD>`__.

#############
Documentation
#############

.. toctree::
   :maxdepth: 1
   :caption: Introduction

   faq

.. toctree::
   :maxdepth: 2
   :caption: Compiler

   compiler-design
   compiler-optimizations

.. toctree::
   :maxdepth: 1
   :caption: Settings

   settings-options
   command-line-arguments

.. toctree::
   :maxdepth: 2
   :caption: Tools

   tools
   tool-compiler
   tool-launcher
   tool-launcher-console
   tool-explorer
   tool-debugger
   tool-boot-image

.. toctree::
   :maxdepth: 1
   :caption: Advanced

   unit-tests
   usb-flash-drive-installation


.. toctree::
   :maxdepth: 1
   :caption: Contribute

   get-involved
   authors
   bsd-license
