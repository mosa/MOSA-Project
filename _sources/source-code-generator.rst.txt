#####################
Source code generator
#####################

Some areas of the compiler are automatically generated from JSON files, such as :doc:`transformations<compiler-transformations>` for example. The **Source code generator** utility exists to assist with this task. It takes those JSON files (found in the ``Source/Docs`` folder) and generates C# code, which is then written to specific files in certain projects, like ``Mosa.Compiler.Framework``.

This utility doesn't take any command line arguments. It requires you to be in the ``bin`` directory to be able to correctly find the ``Source`` directory. To run the utility, simply execute the following commands on any platform:

.. code-block:: bash

	cd bin
	dotnet Mosa.Utility.SourceCodeGenerator.dll

The process won't take very long. The utility doesn't generate any console output, as it writes the contents directly to output files and moves to the next input files. The source code of this utility is very simple, so don't hesitate to check it if you want to learn more about it.
