
.. include:: build-status.rst

############
Introduction
############

MOSA is an open source software project that natively executes .NET applications within a virtual hypervisor or on bare metal hardware!

The MOSA project consists of:

- Compiler - a high quality, multithreaded, cross-platform, optimizing .NET compiler
- Kernel - a small kernel operating system
- Device Drivers Framework - a modular, device drivers framework and device drivers
- Debugger - QEMU-based debugger

**************
Current Status
**************

The target platforms are:

- Intel X86/32-bit (stable)
- Intel X64 (in development)
- ARM v6 (in early development)

The MOSA compiler supports most object and non-object oriented code, including:

- Generic Code (example: List<T>)
- Delegates (static and non-static) and with optional parameters
- Exception Handling (try, finally, and catch code blocks)

The MOSA compiler seeks to provide high quality code generation using the following optimizations:

- Constant Folding and Propagation 
- Strength Reduction optimization
- Dead Code Elimination
- Single Static Assignment (SSA)
- Global Value Numbering / Common Subexpession Elimination
- Sparse Conditional Constant Propagation
- Inlined Expansion
- Loop-Invariant Code Motion
- Null Check Elimination
- Devirtualization
- Block Reordering
- Greedy Register Allocation

See the :doc:`Compiler Optimizations<compiler-optimizations>` for a description of each of these optimization methods. 
