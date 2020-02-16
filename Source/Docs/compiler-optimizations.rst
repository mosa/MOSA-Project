######################
Compiler Optimizations
######################

Optimizations
-------------

Constant Folding and Propagation
  Constant folding is the process of recognizing and evaluating constant expressions at compile time rather than computing them at runtime.

Strength Reduction
  Strength reduction is a compiler optimization where expensive operations are replaced with equivalent but less expensive operations.

Dead Code Elimination
  Dead code elimination is an optimization to remove instructions which does not affect the results of a program.

Static Single Assignment Form
  In Single Static Assignment (SSA) form, all virtual registers may only have one definition. While not an immediate optimization by itself, it enables other optimization opportunities by simultaneously simplifies and improves the results of the other compiler optimizations.

Sparse Conditional Constant Propagation
  Sparse conditional constant propagation is an optimization applied after conversion to static single assignment form (SSA). It simultaneously removes dead code and propagates constants. It can finds more constant values, and thus more opportunities for improvement, than other optimizations.

Global Value Numbering
  Global Value numbering is a technique of determining when two computations in a program are equivalent and eliminating one of them with while preserving the same semantics. 

Inline Expansion
  Inline Expansion replaces a methods call site with the body of the called method. This improves the performance, because calls can be expensive (storing the registers, placing the arguments onto stack, jumping to another location). 

Bit Tracker
  Bit Tracker tracks the known state of bits and value ranges thru the various operations. This may enables other optimizations and shortcuts. 

Block Reordering
  Basic block reordering organizaze block of instructions to maximize the number of fall-through branches.

Greedy Register Allocation
  Greed Register Allocation is a form of register allocation that allocates registers based on live ranges and spill weights. 

Long Expansion
  Expands 64-bit instructions into 32-bit components for platforms without native 64-bit instructions.

Two Pass Optimizations
  This options causes the optimization stages to be executed again, possibility unlocked additional optimizations.

