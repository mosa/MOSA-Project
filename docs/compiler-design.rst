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

Inlined Methods
  Inlines the code of small methods into the caller. This improves the performance, because calls are expensive (Storing the registers, placing the arguments onto stack, jumping to another location). As side effect, inlining methods may increase or decrease the compile file size. For debugging purposes it could be usefull to disable this optimizations (setting correct breakpoint, see real back trace in GDB).

Bit Tracker
  Bit Tracker tracks the known state of bits and value ranges thru the various operations. This enables various optimization and shortcuts. 

Static Single Assignment Form
  TODO: Documentation	

IR Optimization
  TODO: Documentation
  
Sparse Conditional Constant Propagation
  Sparse conditional constant propagation is an optimization applied after conversion to static single assignment form (SSA). It simultaneously removes dead code and propagates constants. It can find more constant values, and thus more opportunities for improvement, than other optimizations.

Long Expansion
  Expands 64-bit instructions into 32-bit components for platforms without native 64-bit instructions.

Value Numbering
  Value numbering is a technique of determining when two computations in a program are equivalent and eliminating one of them with a semantics preserving optimization. 

Two Pass Optimizations
  This options causes the optimization stages to be executed again, possibility unlocked additional optimiztions.

