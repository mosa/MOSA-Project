Frequently Asked Questions (FAQs)
=================================

These are questions we're frequently answering in our IRC channel, the mailing list or which come up during conversations.

What does MOSA stand for?
-------------------------

Managed Operation System Alliance. It was an alliance with the SharpOS project to create a .NET based operating system and toolsets. As operating systems share a lot of common groundwork MOSA aims to standardize interfaces in order to foster portability and to provide both projects with basic implementations of these interfaces.

Who can join?
-------------

Anyone who is interested in operating system or .NET development can join. You have to keep in mind our :doc:`New BSD License <license>` for your contributions though.

What kind of .NET runtime will be available?
--------------------------------------------

We are developing an entirely new runtime as part of the MOSA effort. This runtime is developed along the CIL specifications published by ECMA. We are building our own runtime with pluggable algorithms in order to be very flexible and usable for research.

How is the Cosmos project different than MOSA?
----------------------------------------------

Cosmos is designed to be an operating system toolkit plugin for Visual Studio. The Cosmos toolkit, once installed, integrates with Visual Studio in two significant ways. First, the toolkit introduces a new Cosmos project type that can launch and control the build process. Second, the toolkit integrates with Visual Studio’s debugger and provides break points and watches. The toolkit requires Microsoft’s implementation of the .NET framework to compile a Cosmos operating system.

In comparison, MOSA has no dependencies on any Microsoft’s applications including Visual Studio, the .NET framework or Windows operation system. MOSA can run on Windows, Linux or the Apple’s OSX operating systems. 

Another important difference is Cosmos compiles to Assembly ASM and uses Netwide Assembler, NASM, to finally compile to binary. MOSA compiles directly to binary and has its own linker implementation.

Are Cosmos and MOSA working together?
-------------------------------------

No; Cosmos and MOSA are seperate and independent projects. 
