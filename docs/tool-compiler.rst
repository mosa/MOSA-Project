#############
MOSA Compiler
#############

The **MOSA Compiler** is a command line application used to compile a .NET application to a binary object file.

The compiler is invoked via Command Line:

.. code-block:: console

  Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin Mosa.HelloWorld.x86.exe

Output:

.. code-block:: console

    X:\MOSA-Project\bin>Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin Mosa.HelloWorld.x86.exe
    MOSA Compiler, Version 2.0.0.0.
    Copyright 2020 by the MOSA Project. Licensed under the New BSD License.

    Parsing options...
     > Output file: _main.exe
     > Input file(s): Mosa.HelloWorld.x86.exe
     > Platform: x86

    Compiling ...

    0.56 [0] Compile Started
    0.59 [0] Setup Started
    0.60 [0] Setup Completed
    0.60 [0] Compiling Methods
    2.97 [0] Compiling Methods Completed
    2.97 [0] Finalization Started
    3.06 [0] Linking Started
    3.08 [0] Linking Completed
    3.10 [0] Finalization Completed
    3.10 [0] Compile Completed

Command Line Options
--------------------

[TODO]
