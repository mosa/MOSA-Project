#####################
MOSA Launcher Console
#####################

The **MOSA Launcher Console** is a console application that automates the entire build process including launching of a virtual machine.

.. tip:: Check out the :doc:`MOSA Launcher <tool-launcher>` for a graphic user interface version of this console application.

Usage
-----

A quick example that compiles `Mosa.Demo.TestWorld.x86.dll` demo with `-oMax` (all optimization enabled) and launches it using `Qemu`:

.. code-block:: text

  Mosa.Tool.Launcher.Console.exe -oMax Mosa.Demo.TestWorld.x86.dll

.. code-block:: text

    Current Directory: X:\MOSA-Project-tgiphil\bin
    Compiling: 0.89 secs: Compile Started
    Compiling: 0.94 secs: Compiling Methods
    Compiling: 4.32 secs: Compiling Methods Completed
    Compiling: 4.56 secs: Linking Started
    Compiling: 4.59 secs: Linking Completed
    Compiling: 4.66 secs: Compile Completed
    Generating Image: img
    Launching Application: ..\Tools\QEMU\qemu-system-i386.exe
    Arguments:  -L "..\Tools\QEMU" -cpu qemu32,+sse4.1 -serial null -hda "C:\Users\phil\AppData\Local\Temp\MOSA\Mosa.Demo.TestWorld.x86.img"

Command Line Options
--------------------

See the :doc:`command line arguments<command-line-arguments>` for a list of available options.

Here are the most common options available:

`-autostart-off`
	Does not immediate start the compiler
