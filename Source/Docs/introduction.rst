
.. include:: build-status.rst

############
Introduction
############

MOSA is an open source software project that natively executes .NET applications within a virtual hypervisor or on bare metal hardware!

The MOSA project consists of:

- Compiler - a high quality, multi-threaded, cross-platform, optimizing .NET compiler
- Kernel - a small kernel operating system
- Device Drivers Framework - a modular, device drivers framework and device drivers
- Debugger - QEMU-based debugger

**************
Current Status
**************

The target platforms are:

- 32-bit x86
- 64-bit x86 (x64; in development)
- 32-bit ARMv8+ (in early development)
- 64-bit ARMv8+ (in early development)

The MOSA compiler supports most object and non-object oriented code, including but not limited to:

- Generic Code (e.g. List<T>)
- Delegates (static and non-static) and with optional parameters
- Exception Handling (try, finally, and catch code blocks)

The MOSA compiler seeks to provide high quality code generation using the following optimizations (non-exhaustive):

- Constant Folding and Propagation
- Strength Reduction optimization
- Dead Code Elimination
- Single Static Assignment (SSA)
- Global Value Numbering / Common Subexpression Elimination
- Sparse Conditional Constant Propagation
- Inlined Expansion
- Loop-Invariant Code Motion
- Null Check Elimination
- Devirtualization
- Block Reordering
- Greedy Register Allocation

For an exhaustive list, see :doc:`compiler-optimizations` which also gives a description of each of these optimizations.
