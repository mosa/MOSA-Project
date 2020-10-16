#############
MOSA Debugger
#############

The **MOSA Debugger** is GUI application used to debug a MOSA compiled application using `QEMU Virtual Machine Emulator <https://www.qemu.org>`__.

.. image:: images/mosa-debugger.png

Features
--------------------

* Breakpoints
* Watchpoints
* Register View
* Stack Frame View
* Call Stack View
* Method View
* Source Code View
* Method Parameters View
* Memory Views

Under the hood, **MOSA Debugger** utilitizes the QEMU virtual machine emulator and controls it with native GDB commands.

Usage
------

The **MOSA Debugger** can be launched by executing ``Mosa.Tool.Debugger.exe``. 

In addition, the tool can be launched from the command line with arguments:

.. code-block:: text

	Mosa.Tool.Launcher.exe Mosa.Debugger.x86.exe

Command Line Options
--------------------

See the :doc:`command line arguments<command-line-arguments>` for a list of available options. 
