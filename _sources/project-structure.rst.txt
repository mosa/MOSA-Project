#################
Project structure
#################

MOSA has a lot of projects, which can seem daunting at first. This page will exclusively cover all the types of projects
MOSA has, and explain what they're for.

****************
Mosa.BareMetal.*
****************

They're the demo projects using the **Mosa.Kernel.BareMetal** kernel.

***************
Mosa.Compiler.*
***************

They make up the MOSA compiler. They include the CIL, IR transformations and native code generation stages. You can
learn more about it :doc:`here<compiler-design>`.

*****************
Mosa.DeviceDriver
*****************

It hosts all the device driver implementations, whether they're ISA, PCI, USB, etc...

*****************
Mosa.DeviceSystem
*****************

It mostly contains abstraction classes (like the device driver framework), generic interfaces (like ``IGraphicsDevice``),
and general other miscellaneous utility classes (like ``FrameBuffer32`` or ``ConstrainedPointer``).

It is a dependency of **Mosa.DeviceDriver**.

***************
Mosa.FileSystem
***************

It implements the file system used in MOSA. It currently only supports FAT12 and FAT16.

*********************
Mosa.Kernel.BareMetal
*********************

It's the mainline, platform-agnostic implementation of the MOSA kernel. Platform-specific methods are found in the
``Platform`` class exclusively.

***********************
Mosa.Kernel.BareMetal.*
***********************

They implement the internal, platform-specific functions of **Mosa.Kernel.BareMetal** as **plugs**.

***********
Mosa.Korlib
***********

It's the MOSA implementation of the .NET standard library. It's highly portable and can be used in any project requiring
a custom standard library.

****************
Mosa.Plug.Korlib
****************

While the MOSA implementation of the .NET standard library is portable, it still needs some methods implemented by the
compiler in order to fully benefit from its potentia (known as **intrinsics**). That's where the **Mosa.Plug.Korlib**
project kicks in. It replaces those internal, unimplemented functions with intrinsics via **plugs**. It has a couple of
dependencies on other projects, such as **Mosa.Runtime**. However, it does **not** have dependencies on projects which
are built on a specific platform (like **Mosa.Kernel.BareMetal.x86** for example).

******************
Mosa.Plug.Korlib.*
******************

They're identical to the project above, except they use platform-specific dependencies like **Mosa.Runtime.x86** (in the
case of **Mosa.Plug.Korlib.x86** for example). They're used if e.g. the **plug** must be implemented via
platform-specific instructions.

************
Mosa.Runtime
************

It provides the main entry point for the kernel, as well as other critical classes (like ``Pointer`` and ``GC``) used
throughout MOSA.

**************
Mosa.Runtime.*
**************

They extend **Mosa.Runtime** by providing a few useful native instructions of the specific platform.

***********
Mosa.Tool.*
***********

They're the tools provided by the MOSA project. You can learn more about them in the Tools section.

**************
Mosa.Templates
**************

It provides the base NuGet template used when creating a new project. Inside the project exists a solution
(``Mosa.Starter.sln``) whose project structure follows the demos' (1 library with the main OS, multiple executable
stubs for each platform).

**************
Mosa.Utility.*
**************

They're, in most cases, libraries (but can also be executables like ``Mosa.Utility.SourceCodeGenerator``) aimed at
sharing common code for other projects. They can also prove useful if you want to make your own set of tools for example.

****************
Mosa.UnitTests.*
****************

They host the bare metal unit tests, which are automatically triggered in pull requests and main repository commits, but
can also :doc:`be triggered manually<unit-tests>`.

****************
Mosa.Workspace.*
****************

They're simple playgrounds used for testing pretty much anything, such as new compiler optimizations or changes. At
their core, they're simply stripped out kernels.
