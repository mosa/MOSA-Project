######################
Compiler optimizations
######################

Optimizations
-------------

Constant Folding and Propagation
	Constant folding is the process of recognizing and evaluating constant expressions at compile time rather than
	computing them at runtime.

Strength Reduction
	Strength reduction is a compiler optimization where expensive operations are replaced with equivalent but less
	expensive operations.

Dead Code Elimination
	Dead code elimination is an optimization to remove instructions which does not affect the results of a program.

Static Single Assignment (SSA)
	In SSA form, all virtual registers may only have one definition. While not an immediate optimization by itself, it
	enables other optimization opportunities by simultaneously simplifying and improving the results of the other
	compiler optimizations.

Sparse Conditional Constant Propagation
	Sparse conditional constant propagation is an optimization applied after conversion to SSA form. It simultaneously
	removes dead code and propagates constants. It can find more constant values, and thus provides more opportunities
	for improvement.

Global Value Numbering
	Global value numbering is a technique of determining when two computations in a program are equivalent and
	eliminating one of them while preserving the same semantics.

Inline Expansion
	Inline expansion replaces a method's call site with the body of such method. This improves performance, because
	calls can be expensive (storing the registers, placing the arguments onto stack, jumping to another location).

Bit Tracker
	Bit tracker tracks the known state of bits and value ranges through the various operations. This may enables other
	optimizations and shortcuts.

Basic Block Reordering
	Basic block reordering organizes block of instructions to maximize the number of fall-through branches.

Greedy Register Allocation
	Greedy register allocation assigns CPU registers based on live ranges and spill weights. It is a form of register
	allocation.

Long Expansion
	Long expansion transforms 64-bit instructions into 32-bit instructions for platforms without native 64-bit
	instructions. This may result in further optimization opportunities.

Devirtualization
	Devirtualization is an optimization where virtual method calls are translated into faster static method calls. It
	can happen when the compiler can statically determine at compile-time which method is called, so it can produce a
	direct call to that method instead, or even inline it.

Null Check Elimination
	Null check elimination is an optimization that removes null checks instructions when the compiler can statically
	determine at compile-time that the object reference is never null.

Two Pass Optimizations
	This option causes the optimization stages to be executed again, possibly unlocking additional optimizations.
