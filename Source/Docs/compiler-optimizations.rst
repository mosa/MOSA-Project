######################
Compiler Optimizations
######################

Optimizations
-------------

Constant Folding and Propagation
	Constant Folding is the process of recognizing and evaluating constant expressions at compile time rather than computing them at runtime.

Strength Reduction
	Strength Reduction is a compiler optimization where expensive operations are replaced with equivalent but less expensive operations.

Dead Code Elimination
	Dead code elimination is an optimization to remove instructions which does not affect the results of a program.

Static Single Assignment Form
	In Single Static Assignment (SSA) form, all virtual registers may only have one definition. While not an immediate optimization by itself, it enables other optimization opportunities by simultaneously simplifies and improves the results of the other compiler optimizations.

Sparse Conditional Constant Propagation
	Sparse Conditional Constant Propagation is an optimization applied after conversion to static single assignment form (SSA). It simultaneously removes dead code and propagates constants. It can finds more constant values, and thus more opportunities for improvement, than other optimizations.

Global Value Numbering
	Global Value Numbering is a technique of determining when two computations in a program are equivalent and eliminating one of them with while preserving the same semantics. 

Inline Expansion
	Inline Expansion replaces a methods call site with the body of the called method. This improves the performance, because calls can be expensive (storing the registers, placing the arguments onto stack, jumping to another location). 

Bit Tracker
	Bit Tracker tracks the known state of bits and value ranges thru the various operations. This may enables other optimizations and shortcuts. 

Block Reordering
	Basic Block Reordering organizaze block of instructions to maximize the number of fall-through branches.

Greedy Register Allocation
	Greedy Register Allocation assigns cpu registers based on live ranges and spill weights.

Long Expansion
	Long Expansion is the transformation of 64-bit instructions into 32-bit instructions for platforms without native 64-bit instructions. This may result in further optimization opportunities.

Devirtualization
	Devirtualization is an optimization where virtual method calls are translated into faster static method calls. Devirtualization can happen when the compiler can statically determine at compile which method should be called, so it can produce a direct call to that method, or even inline it. 

Null Check Elimination
	Null Check Elimination is an optimization that removes null checks instructions when the compiler can statically determine at compile that the object reference is never null. 

Two Pass Optimizations
	This options causes most optimization stages to be executed again, possibility unlocked additional optimizations.
