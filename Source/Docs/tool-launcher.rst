#############
MOSA Launcher
#############

The **MOSA Launcher** is GUI application used to select various compiler and build options. Once options are select, the tool automates the entire build process including launching of a virtual machine.

.. image:: images/mosa-launcher.png

.. tip:: Check out the :doc:`MOSA Launcher Console <tool-launcher-console>` for a console version of this application.

Usage
-----

The **MOSA Launcher** can be launched by executing ``Mosa.Tool.Launcher``.

In addition, the tool can be launched from the command line with arguments:

.. code-block:: text

	Mosa.Tool.Launcher Mosa.Demo.HelloWorld.x86.dll


Command Line Options
--------------------

See the :doc:`command line arguments<command-line-arguments>` for a list of available options.

Here are the most common options available:

`-autostart-off`
	Does not immediate start the compiler
