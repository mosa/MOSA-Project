###############
Compiler Design
###############

The MOSA Compiler Framework is based on the idea of a pipeline. Every method is compiled by a method compiler, that specifies the pipeline to use for compilation. Such a pipeline may consist of any number of compilation stages. These stages can be grouped into various kinds:

- Compiler Frontends - Create an instruction stream from a source specific representation, such as CIL or Java byte code
- Transformation Stages - Transform the instruction stream between various representations
- Optimization Stages - Various stages intended to optimize the code to execute faster
- Register Allocation - Allocate architecture specific registers to operands used in the instruction stream
- Compiler Backends - Generate code from the intermediate and architecture specific representations

The compiler framework provides predefined pieces of this pipeline. Some parts, especially the code generation, are provided by the architecture specific stages, such as for the x86 platform.

.. rubric:: Intermediate representations

The compiler framework uses a linear intermediate representation to transform the source program text into machine code. There are several levels of intermediate representations before code generation. These are:

- CIL - Common Intermediate Language
- IR - High-Level Intermediate Representation
- MIR - Machine specific Intermediate Representation

During compilation of an CIL method the instructions are represented by CIL instruction classes and moving forward, the linear instruction stream is modified to use instructions from the intermediate representation. In some cases an instruction from the intermediate representation can not be emitted directly to machine code and it is replaced by a sequence of machine specific instruction objects. The machine specific instruction classes are provided by the appropriate platform. There are other uses for machine specific instruction classes, but the main use is effective code generation.

Compiler Optimizations
----------------------

BitTracker
  BitTracker tracks the bits in virtual registers thru the various instructions. Either, set, clear or unknown per bit. With all the other optimization enabled, it doesn’t do much.
