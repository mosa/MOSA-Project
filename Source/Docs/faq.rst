Frequently Asked Questions (FAQs)
=================================

These are questions we're frequently answering in our Discord server or which come up during conversations.

What does MOSA stand for?
-------------------------

It stands for **Managed Operating System Alliance**. It was originally an alliance with the SharpOS project to create a
.NET based operating system and toolsets. As operating systems share a lot of common groundwork, MOSA aims to
standardize interfaces in order to foster portability and to provide both projects with basic implementations of these
interfaces.

Who can join?
-------------

Anyone who is interested in operating system or .NET development can join. You have to keep in mind our
:doc:`New BSD License <license>` for your contributions though.

What kind of .NET runtime will be available?
--------------------------------------------

We are developing an entirely new runtime as part of the MOSA effort. This runtime is developed along the CIL
specifications published by ECMA. We are building our own runtime with pluggable algorithms in order to be very flexible
and usable for research.

How is the Cosmos project different than MOSA?
----------------------------------------------

Cosmos is designed to be an operating system toolkit plugin for Visual Studio. The Cosmos toolkit, once installed,
integrates with Visual Studio in two significant ways. First, the toolkit introduces a new Cosmos project type that can
launch and control the build process. Second, the toolkit integrates with Visual Studio’s debugger and provides
breakpoints and watches. The toolkit requires Microsoft’s implementation of the .NET framework to compile a Cosmos
operating system.

In comparison, MOSA has no dependencies on any of those applications like Visual Studio or Windows. MOSA can run on all
the platforms where .NET runs!

Another important difference is Cosmos compiles to Assembly and uses an intermediate assembler to finally compile to
machine code. On the other hand, MOSA compiles directly to binary code and has its own linker implementation.

Are Cosmos and MOSA working together?
-------------------------------------

Recently, there have been talks to integrate the MOSA compiler into Cosmos, thus potentially being able to implement
more features in the MOSA compiler while also providing more stability and performance for Cosmos. While nothing has
come out of this yet, you can express your opinion on this in our Discord in #cosmos-integration and in the Cosmos
Discord in #dev-mosa-cosmos.
