###############
Compiler design
###############

The MOSA Compiler framework is designed around two pipelines, each with multiple stages, and a type system.

Type System
-----------

MOSA uses a very similar type system to the one found in .NET, except it's specific to the MOSA compiler. All types,
methods, etc.. get translated from their type system to MOSA's. This is the very first step before the compiler actually
runs.

Compiler Pipeline
-----------------

It's the primary pipeline that executes the steps necessary to compile an application. It:
- Creates the type system
- Compiles each method
- Links the executable
- Emits the final object file

One stage executes the **Method Compiler Pipeline** for each method in the application.

Method Compiler Pipeline
------------------------

It's used to compile a single method by progressively lowering the high level instruction representation to the final
opcode instructions of the target platform. This pipeline is executed at least once for all methods in the application.

All the stages can be grouped into one or more of these categories:

Decoding Stage
	Creates an instruction stream for a method from the source representation, such as a method from a .NET application.

Transformation Stages
	Transforms the instruction stream between various representations, usually from a higher level to a lower level
	representation.

Optimization Stages
	Transforms instructions with the intention to make the code execute faster.

Register Allocation Stage
	Allocates the virtual registers in the instruction stream to platform-specific physical registers.

Platform-Specific Transformation Stages
	Transforms the instructions in the stream into specific platform opcodes.

Intermediate Representations
----------------------------

The compiler framework uses a linear intermediate representation (v.s. an expression tree) to transform the source
application into binary machine code. There are several levels of intermediate representations before code generation.
These are:

- Common Intermediate Language (CIL)
- High-Level Intermediate Representation (IR)
- Machine specific Intermediate Representation (MIR)

During compilation of a CIL method, the instructions are represented by CIL instruction classes, and moving forward, the
linear instruction stream is modified to use instructions from the intermediate representation. In some cases an
instruction from the intermediate representation can not be emitted directly to machine code and it is replaced by a
sequence of machine specific instructions. The machine specific instruction classes are provided by the appropriate
platform.
