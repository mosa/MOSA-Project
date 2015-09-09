# IR Optimizations

The MOSA Compiler implements many common compiler optimizations. The following describes some of the optimizations performed by the compiler's IR Optimization stage (implemented in IROptimizationStage.cs). As its name implies, it operates on the immediate representation (IR) instruction level. It can work on IR instructions in SSA form or not; however, it will perform more optimizations when the instructions are in SSA form. The stage is optional and can be enabled or disabled.

# Iterative Transformations

Since optimization transformations can lead to further optimizations, the process of matching and applying optimizations is iterative. Rather than perform multiple passes over all instructions looking for transformations opportunities, the MOSA compiler uses worklists to queue variables and instructions for transformation matching. Initially, the worklist is populated with all variables and instructions. As variables and instructions are transformed, they are added back to the worklist. The worklist is processed until it is empty. The worklist is very efficient since it avoids duplicate work that a multi-pass process would occur.

## Constant Propagation

Constant propagation is the process of substituting the values of known constants in expressions at compile time. Consider the following pseudocode:

````
a = 10
b = 20

c = a + b
````
Propagating a and b yields

````
a = 10
b = 20

c = 10 + 20
````
_Note: The constant assessments to variables a and b remain unchanged during constant propagation. The dead code elimination will remove them if they are not used elsewhere._

## Constant Folding

Constant folding is the process of evaluating constant expressions and substituting the result. For example:

````
c = 10 + 20
````
Constant folding evaluates 10 + 20 and substitutes 30 in place of that expression.
````
c = 30
````

MOSA implements the following types of constant folding integer operations: 
* 	addition (a + b)
* 	subtraction (a - b)
* 	multiplication (a * b)
* 	division (a / b), except when b == 0
* 	logical and (a & b)
* 	logical or (a | b)
* 	logical exclusive or (a ^ b)
* 	arithmetic right shift (a >> b)
* 	left and right shits (a << b and a >> b)

_Note: Floating point values are not folded._

## Arithmetic Simplification 

Arithmetic simplification replace expensive arithmetic operations by cheaper ones. 

MOSA implements the following arithmetic simplifications:

Original | Optimized | Condition
---------|-----------|-----------
c = a - a | c = 0
c = a - 0 | c = a
c = a + 0 | c = a
c = a * 0 | c = 0
c = a * 1 | c = a
c = a / 1 | c = a
c = 0 / b | c = 0 | where b != 0 and b is a constant
c = a (or)  0 | c = a
c = a & 0 | c = 0
c = a & 0xFFFFFFFF | c = a | where c is 32-bits
c = a & 0xFFFFFFFFFFFFFFFF | c = a | where c is 64-bits
c = a >> 0 | c = a
c = a << 0 | c = a
c = 0 << b | c = 0
c = 0 >> b | c = 0
c = a * b | c << s | where b is a power of 2 and s = log 2 (b)
c = a / b | c >> s | where b is a power of 2, the division is unsigned and s = log 2 (b)

_Note: More arithmetic simplifications can be added._

## Move Constants to the Right

This transformation moves constants from the left side of an operation to the right, when the operation is commutative, such as addition and multiplication. While this is not an optimization per se, it simplifies the operation and operands for other optimizations. Examples:

Original | Transformation | Condition
---------|-----------|-----------
C * a | a * C | where C is a constant
C + a | a + C | where C is a constant

## Dead Code Elimination

Dead code elimination is the removal of code that is unreachable or whose result is never used in any other computation. Dead code is commonly created by other optimization transformations, such as constant propagation and arithmetic simplification. 

The MOSA compiler tracks the assignment and use of every variable. When an variable has no uses (its use count is zero), the operation that assigned the operand is removed.

Example:

````
public int DoSomething(int a, int b)
{
	x = a + b;
	z = y * b;	// variable z is never used, and therefore the entire multiplication operation can be eliminated
	return x;
}
````

