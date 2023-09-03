######################
MOSA Project Structure
######################

MOSA has a lot of projects, which can seem daunting at first. This page will exclusively cover all the types of projects that MOSA has, and explain what they are for.

********************************
Mosa.BareMetal.*
********************************

Those are the demo projects using the **Mosa.Kernel.BareMetal** kernel.

***************
Mosa.Compiler.*
***************

Those projects make up the MOSA compiler. They include the CIL, IR transformations and native code generation stages. You can learn more about it :doc:`here<compiler-design>`.

*****************
Mosa.DeviceDriver
*****************

This project hosts all the drivers MOSA offers, whether they're ISA, PCI, USB, etc...

*****************
Mosa.DeviceSystem
*****************

This project contains all kinds of miscelleanous classes and utilities, useful to device drivers and other stuff. It can almost be fully used outside MOSA (it does contain a few classes related to MOSA, like ``ConstrainedPointer``) and can serve as a great utilities project. For example, it contains classes for generating a VDI header or VHD footer, MBR generation and some PCI utilities. But it contains much more!

It is a dependecy of **Mosa.DeviceDriver**.

***************
Mosa.FileSystem
***************

This project is the MOSA implementation for the file system. It currently only supports FAT32, but it aims to support more in the future.

*********************
Mosa.Kernel.BareMetal
*********************

This project is the mainline, platform-agnostic implementation of the MOSA kernel.

***********************
Mosa.Kernel.BareMetal.*
***********************

Those projects implement the internal, platform-specific functions of **Mosa.Kernel.BareMetal** as **plugs**.

***********
Mosa.Korlib
***********

This project is the MOSA implementation of the .NET standard library. While it is currently missing a lot of features, it is highly portable and can be used in any project requiring a custom core library.

****************
Mosa.Plug.Korlib
****************

While the MOSA implementation of the .NET standard library is portable, it does need some functions implemented by the compiler in order to fully benefit from its potential. This is where this project comes in. It replaces those internal, unimplemented functions with **plugs**. It has a couple of dependencies on other projects, including **Mosa.Runtime** for example. However, it does **not** have dependencies on projects which are built on a specific platform (like **Mosa.Kernel.BareMetal.x86** for example).

******************
Mosa.Plug.Korlib.*
******************

Those platform-specific projects are identical to the one above, except they do use platform-specific dependencies like **Mosa.Runtime.x86** in the case of **Mosa.Plug.Korlib.x86**. Those are used if the **plug** must be implemented via platform-specific instructions for example.

************
Mosa.Runtime
************

This project provides the boot function and process for the kernel, which the compiler integrates in the final binary.

However, it also provides classes like Pointer or GC.

**************
Mosa.Runtime.*
**************

Those projects help in providing additional functions for using native instructions of the specific platform.

***********
Mosa.Tool.*
***********

Those projects are the tools provided by the MOSA project. You can learn more about them in the Tools section.

**************
Mosa.Utility.*
**************

Those projects provide common code for other projects to use, such as the tools.

****************
Mosa.UnitTests.*
****************

Those projects host the unit tests, which are automatically triggered in pull requests and main repository commits, but can also :doc:`be triggered manually<unit-tests>`.

****************
Mosa.Workspace.*
****************

Those projects are simple playgrounds, which are there to be able to test stuff, such as new compiler optimizations or changes. They're basically stripped out kernels used to test specific stuff.

**************
Other projects
**************

If some projects are not mentioned here, it's probably because they're miscellaneous and/or do not fit in any of the categories cited above.
