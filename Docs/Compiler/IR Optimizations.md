# IR Optimizations

The MOSA Compiler implements many common compiler optimizations. The following describes some of the optimizations performed by the IR Optimization Stage (implemented in IROptimizationStage.cs). As its name implies, it operates on the immediate representation (IR) instruction level. It can work on IR instructions in SSA form or not; however, it will perform more optimization transformation when the code is in SSA form. The stage is optional and can be enabled or disabled.

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

_Note: More arithmetic simplification can be added._


