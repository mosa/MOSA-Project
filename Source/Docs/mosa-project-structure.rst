######################
MOSA Project Structure
######################

MOSA has a lot of projects, which can seem daunting at first. This page will exclusively cover all the types of projects that MOSA has, and explain what they are for.

********************************
Mosa.Demo.* and Mosa.BareMetal.*
********************************

Those are demo projects, for **Mosa.Kernel**.* and **Mosa.Kernel.BareMetal**.* projects respectively.

***************
Mosa.Compiler.*
***************

Those projects make the MOSA compiler. You can learn more about it :doc:`here<compiler-design>`.

*****************
Mosa.DeviceDriver
*****************

This project hosts all the drivers MOSA offers. They're not required to run a demo, nor are they required if you want to make your own drivers.

*****************
Mosa.DeviceSystem
*****************

This project contains all kinds of miscelleanous classes and utilities, useful to device drivers and other stuff. It can almost be fully used outside MOSA (it does contain a few classes related to MOSA, like ``ConstrainedPointer``) and can serve as a great utilities project. For example, it contains classes for generating a VDI header or VHD footer, MBR generation and some PCI utilities. But it contains much more!

It's also not required to run a demo, however it is a dependecy of **Mosa.DeviceDriver**.

***************
Mosa.FileSystem
***************

This project is the MOSA implementation for the file system. It currently only supports FAT32, but it aims to support more in the future.

*************
Mosa.Kernel.*
*************

Those projects are the MOSA kernel implementations for various platforms. They're not strictly required to create an OS with MOSA though, if you want to implement it yourself.

*********************
Mosa.Kernel.BareMetal
*********************

This project is part of the :doc:`BareMetal<baremetal>` experiment, and aims to unify platform-agnostic code into one project, for other projects to use. It provides a **Platform** class, in which specific BareMetal kernel implementations can implement those functions via **plugs**.

***********************
Mosa.Kernel.BareMetal.*
***********************

Those projects implement the internal, unimplemented functions of **Mosa.Kernel.BareMetal** as **plugs**.

***********
Mosa.Korlib
***********

This project is the MOSA implementation of the .NET standard library. While it is currently missing a lot of features, it is highly portable and can be used in any project. You can also use your own implementation if you want to.

***************
Mosa.Platform.*
***************

Those projects host the code generation stages of their respective platform for the MOSA compiler.

****************
Mosa.Plug.Korlib
****************

While the MOSA implementation of the .NET standard library is portable, it does need some functions implemented by the compiler in order to fully benefit from its potential. This is where this project comes in. It replaces those internal, unimplemented functions with **plugs**. It has a couple of dependencies on other projects, including **Mosa.Runtime** for example. However, it does **not** have dependencies on projects which are built on a specific platform (like **Mosa.Kernel.x86** for example).

******************
Mosa.Plug.Korlib.*
******************

Those platform-specific projects are identical to the one above, except they do use platform-specific dependencies like **Mosa.Kernel.x86** in the case of **Mosa.Plug.Korlib.x86**. Those are used if the **plug** must be implemented via a platform-specific standard or device.

************
Mosa.Runtime
************

This project provides the boot function and process for the kernel, which the compiler integrates in the final binary. It is therefore strictly required, even if you don't use any of the other projects in your OS.

However, it also provides classes like Pointer or GC.

**************
Mosa.Runtime.*
**************

Those projects help in providing additional functions for using native instructions of the specific platform. For the platform you're using, that project is not strictly required to create an OS with MOSA.

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

Those projects host the unit tests, which are triggered in pull requests and main repository commits, but can also :doc:`be triggered manually<unit-tests>`.

****************
Mosa.Workspace.*
****************

Those projects are simple playgrounds, which are there to be able to test stuff, such as new compiler optimizations or changes. They're basically stripped out kernels used to test specific stuff.

**************
Other projects
**************

If some projects are not mentioned here, it's probably because they're miscelleanous and/or do not fit in any of the categories cited above.