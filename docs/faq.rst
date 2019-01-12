Frequently Asked Questions (FAQs)
=================================

These are questions we're frequently answering in our IRC channel, the mailing list or which come up during conversations.

What does MOSA stand for?
-------------------------

Managed Operation System Alliance. It was an alliance with the SharpOS project to create a .NET based operating system. As operating systems share a lot of common groundwork MOSA aims to standardize interfaces in order to foster portability and to provide both projects with basic implementations of these interfaces.

.. rubric:: Who can join?

Anyone who is interested in operating system or .NET development can join the alliance. You have to keep in mind our [[New BSD License|License]] for your contributions though.

What was the reason for creating MOSA?
--------------------------------------

The reason is to get the independent .NET operating system projects to work together and agree on some specifications. If any of these projects want to succeed, they'll need driver and developer support. Hardware developers will not write three drivers that basically do the same thing. So the reason for MOSA is simple: We don't want to waste time doing the same things twice or more.

Do you require the use of the MOSA kernel in depending projects?
----------------------------------------------------------------

No. We provide default implementations for kernel components, but there will not be another MOSA kernel. We want to share not embrace. Our policies clearly indicate that we develop kernel components whose primary purpose is to prove the specifications are viable and that they can be reused by projects developing along the MOSA specification.

What do your specifications describe?
--------------------------------------

There are various specifications in development. They have a textual part to them and an .NET interface that can be used by clients to communicate with various implementations of these services.

Why do you require interfaces for kernel services?
--------------------------------------------------

Because interfaces give us a lot of advantages. You can create proxies or adapters for them and using interfaces there's no definition of the kernel structure. Using this approach and the services offered by the .NET platform our services can be reused in nano-, micro- or macrokernels.

Is this based on Unix, Windows or Posix?
----------------------------------------

None. In our opinion it doesn't make sense to develop an operating system from scratch that doesn't take advantage of the features the .NET platform provides. Our specifications do take the same role as POSIX does in the Unix world though.

What advantages of .NET are you talking about?
----------------------------------------------

Self-Describing: Our idea of device drivers is to make them fully self-contained and self-describing. Classical operating system need metadata in the form of external files or kernel descriptor entries. Our drivers are marked with attributes that indicate the devices they support.
Common Intermediate Language: The operating system, its services, the drivers and apps will all be compiled to the CIL by the source code compilers and compiled to native code by the MOSA compiler. We ensure runtime safety and security with our compiler. We do not allow execution of untrusted code or unsafe code.

What about legacy (non-.NET) apps?
----------------------------------

We will not support native code applications on the MOSA kernel itself. A project, which uses the MOSA components may do so. We think that virtualization is the more appropriate way to run native applications on a managed kernel. The MOSA compiler will not support P/Invoke calls natively.

What kind of .NET runtime will be available?
--------------------------------------------

We are developing an entirely new Runtime as part of the MOSA effort. This runtime is developed along the CIL specifications published by ECMA. We are building our own runtime with pluggable algorithms in order to be very flexible and usable for research.

Which .NET assemblies are available?
------------------------------------

There are plans to reuse the assemblies provided by the .NET Core where appropriate. We will adapt their code and replace their system calls and native code interfaces with our specification based approach.

Is this based on Microsoft Singularity?
---------------------------------------

No. We do not share or reuse any code from Microsoft Singularity. Our code is developed from scratch and our developers have invested a lot of time in the design.

Why not reuse the Mono runtime or the SSCLI?
--------------------------------------------

The mono runtime is developed in C - we do not want unsafe code or code not compiled to the CLI. The SSCLI can not be used for the same reasons and additionally again due to restrictive licensing.

How is the Cosmos project different than MOSA?
----------------------------------------------

Cosmos is designed to be an operating system toolkit plugin for Visual Studio. The Cosmos toolkit, once installed, integrates with Visual Studio in two significant ways. First, the toolkit introduces a new Cosmos project type that can launch and control the build process. Second, the toolkit integrates with Visual Studio’s debugger and provides break points and watches. The toolkit requires Microsoft’s implementation of the .NET framework to compile a Cosmos operating system.

In comparison, MOSA has no dependencies on any Microsoft’s applications including Visual Studio, the .NET framework or Windows operation system. MOSA can run on Windows, Linux or the Apple’s OSX operating systems. 

Another important difference is Cosmos compiles to Assembly ASM and uses Netwide Assembler, NASM, to finally compile to binary. MOSA compiles directly to binary and has its own linker implementation.

Are Cosmos and MOSA working together?
-------------------------------------

No; Cosmos and MOSA are independent projects. 
