###############
Compiler Design
###############

The MOSA Compiler framework is designed around two pipelines each with multiple stages.  

Compiler Pipeline
-----------------

**Compiler Pipeline** is the primary pipeline that executes the steps necessary to compile an application, such as building the type system, compiling each method, linking, and emitting the final object file. One of the stages in this pipeline executes the **Method Compiler Pipeline** for each method in the application.

Method Compiler Pipeline
------------------------

The **Method Compiler Pipeline** is used to compile a single method by progressively lowers the high level instruction representation to the final opcode instructions of the target platform. This pipeline is executed at least once for all methods in the application. All the stages can be grouped into these five categories:

- Decoding Stage 
	Creates an instruction stream for a method from the source representation, such as a method from a .NET application 
- Transformation Stages
	Transforms the instruction stream between various representations, usually from a higher level to a lower level representations
- Optimization Stages
	Transforms instructions intended to optimize the code to execute faster
- Register Allocation Stage
	Allocates the virtual registers in the instruction stream to platform specific physical registers 
- Platform Transformation Stages
	Transforms the instructions in the stream into specific platform opcodes

The compiler framework provides predefined pieces of this pipeline. Some parts, especially the code generation, are provided by the architecture specific stages, such as for the x86 platform.

Intermediate Representations
----------------------------

The compiler framework uses a linear intermediate representation (vs an expression tree) to transform the source application into machine code. There are several levels of intermediate representations before code generation. These are:

- Common Intermediate Language (CIL)
- High-Level Intermediate Representation (IR)
- Machine specific Intermediate Representation (MIR) 
	
During compilation of an CIL method the instructions are represented by CIL instruction classes and moving forward, the linear instruction stream is modified to use instructions from the intermediate representation. In some cases an instruction from the intermediate representation can not be emitted directly to machine code and it is replaced by a sequence of machine specific instructions. The machine specific instruction classes are provided by the appropriate platform.
