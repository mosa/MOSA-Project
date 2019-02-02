##################
Mosa Compiler Tool
##################

The Mosa Compiler can also invoked via Command Line:

.. code-block:: text
  
  Mosa.Tool.Compiler.exe -o Mosa.HelloWorld.x86.bin -a x86 --mboot v1 --x86-irq-methods --base-address 0x00500000 mscorlib.dll Mosa.Plug.Korlib.dll Mosa.Plug.Korlib.x86.dll Mosa.HelloWorld.x86.exe

Sample for launcher.json, when using Visual Studio Code:

.. code-block:: js

  "request": "launch",
  "program": "${workspaceRoot}/../bin/Mosa.Tool.Compiler.exe",
  "args": ["-o", "Mosa.HelloWorld.x86.bin", "-a", "x86", "--mboot", "v1", "--x86-irq-methods", "--base-address", "0x00500000", "mscorlib.dll", "Mosa.Plug.Korlib.dll", "Mosa.Plug.Korlib.x86.dll", "Mosa.HelloWorld.x86.exe"],
