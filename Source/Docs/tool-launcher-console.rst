################
Console launcher
################

The **MOSA Console Launcher** is a console application that automates the entire build process including launching of a virtual machine.

.. tip:: Check out the :doc:`launcher <tool-launcher>` for a GUI version of this tool.

Usage
-----

Here's a quick example that compiles ``Mosa.BareMetal.HelloWorld.x86.dll`` demo with ``-oMax`` (all optimizations enabled) and launches it using ``QEMU``:

.. code-block:: text

  Mosa.Tool.Launcher.Console -oMax Mosa.BareMetal.HelloWorld.x86.dll

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
    Arguments:  -L "..\Tools\QEMU" -cpu qemu32,+sse4.1 -serial null -hda "C:\Users\phil\AppData\Local\Temp\MOSA\Mosa.BareMetal.HelloWorld.x86.img"

Command Line Options
--------------------

See the :doc:`command line arguments<command-line-arguments>` for a list of available options.
