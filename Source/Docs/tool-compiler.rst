#############
MOSA Compiler
#############

The **MOSA Compiler** is a console application used to compile a .NET application to a binary object file.

The compiler is invoked via Command Line:

.. code-block:: console

  Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin Mosa.HelloWorld.x86.exe

Output:

.. code-block:: console

    X:\MOSA-Project\bin>Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin Mosa.HelloWorld.x86.exe
    MOSA Compiler, Version 2.0.0.0.
    Copyright 2020 by the MOSA Project. Licensed under the New BSD License.

    Parsing options...
     > Output file: Mosa.HelloWorld.x86.bin
     > Input file(s): Mosa.HelloWorld.x86.exe
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

[TODO]