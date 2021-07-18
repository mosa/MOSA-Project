#############
MOSA Compiler
#############

The **MOSA Compiler** is a console application used to compile a .NET application to a binary object file.

Usage
-----

The compiler is invoked via Command Line:

.. code-block:: console

	Mosa.Tool.Compiler.exe -o Mosa.Demo.HelloWorld.x86.bin Mosa.Demo.HelloWorld.x86.dll

Output:

.. code-block:: console

	X:\MOSA-Project\bin>Mosa.Tool.Compiler.exe -o Mosa.Demo.HelloWorld.x86.bin Mosa.Demo.HelloWorld.x86.dll
	MOSA Compiler, Version 2.0.0.0.
	Copyright 2020 by the MOSA Project. Licensed under the New BSD License.

	Parsing options...
	 > Output file: Mosa.Demo.HelloWorld.x86.bin
	 > Input file(s): Mosa.Demo.HelloWorld.x86.dll
	 > Platform: x86

	Compiling ...

	0.38 [0] Compile Started
	0.41 [0] Setup Started
	0.42 [0] Setup Completed
	0.42 [0] Compiling Methods
	2.79 [0] Compiling Methods Completed
	2.79 [0] Finalization Started
	2.88 [0] Linking Started
	2.89 [0] Linking Completed
	2.92 [0] Finalization Completed
	2.92 [0] Compile Completed

Command Line Options
--------------------

See the :doc:`command line arguments<command-line-arguments>` for a list of available options.

