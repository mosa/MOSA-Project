######################
Compiler Optimizations
######################

Optimizations
-------------

Basic Optimizations
  Basic optimizations are a family of common types of optimizations, such as Constant Folding, Strength Reduction, Simplification, and many others.
  
Bit Tracker
  Bit Tracker tracks the known state of bits and value ranges thru the various operations. This enables various optimization and shortcuts. 

Long Expansion
  Expands 64-bit instructions into 32-bit components for platforms without native 64-bit instructions.

Inline Expansion
  Inline Expansion replaces a methods call site with the body of the called method. This improves the performance, because calls can be expensive (storing the registers, placing the arguments onto stack, jumping to another location). 

Static Single Assignment Form
  Transforms instructions to Single Static Assignment (SSA) form. In SSA form, all virtual registers may only have one definition. This is not an optimization by itself, but this form enables other optimization opportunities in other types of optimizations.

Sparse Conditional Constant Propagation
  Sparse conditional constant propagation is an optimization applied after conversion to static single assignment form (SSA). It simultaneously removes dead code and propagates constants. It can finds more constant values, and thus more opportunities for improvement, than other optimizations.

Value Numbering
  Value numbering is a technique of determining when two computations in a program are equivalent and eliminating one of them with while preserving the same semantics. 

Two Pass Optimizations
  This options causes the optimization stages to be executed again, possibility unlocked additional optimizations.

