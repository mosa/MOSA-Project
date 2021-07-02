########################
Compiler Transformations
########################

The MOSA compiler utilitizes a datebase of transformations to represent specific types of optimizations. These transformation are described in json formatted text files to make it easy to add new optimizations to the compiler.

A special parser program, called ``Mosa.Utility.SourceCodeGenerator``, reads the text files and translates them into C# source code files, which are then included and compiled into the MOSA compiler.

These files are located at ``Source\Data\IR-Optimization-*.json``.

Transformation
==============

Each tranformation has three functional parts: **Expression**, **Filter**, and **Rule**.

The **expression** describes the required expression tree necessary in order for the rule to execute. The **filter** describes the specific attributes of operands in the expression tree that must also be satified for the rule to execute. And the **rule** represents the new, replacement expression tree.

Example #1
----------

Here's an example of a constant folding optimization where an add operation with two constants are translated into a simple move operation.

.. code-block:: bash

		{
			"FamilyName": "IR",
			"Type": "ConstantFolding",
			"Name": "Add32",
			"SubName": "",
			"Expression": "IR.Add32 a b",
			"Filter": "IsResolvedConstant(a) & IsResolvedConstant(b)",
			"Result": "(IR.Move32 [Add32(To32(a),To32(b))])"
		}

The first four fields, FamilyName, Type, Name, and SubName, represent a unique name for the transformation.

This is translated into the following C# code:

.. code-block:: csharp

	public sealed class Add32 : BaseTransformation
	{
		public Add32() : base(IRInstruction.Add32)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			if (!IsResolvedConstant(context.Operand1))
				return false;

			if (!IsResolvedConstant(context.Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			var result = context.Result;

			var t1 = context.Operand1;
			var t2 = context.Operand2;

			var e1 = transformContext.CreateConstant(Add32(To32(t1), To32(t2)));

			context.SetInstruction(IRInstruction.Move32, result, e1);
		}
	}


Example #2
----------

Here's an example of a rewrite optimization where three operations are reduce to two:

.. code-block:: bash

		{
			"FamilyName": "IR",
			"Type": "Rewrite",
			"Name": "And32",
			"SubName": "Not32Not32",
			"Expression": "(IR.And32 (IR.Not32 a) (IR.Not32 b))",
			"Filter": "",
			"Result": "(IR.Not32 (IR.Or32 a b))"
		}

		
Example #3
----------

Here's an example of a strength reduction optimization where a multiplication operation by a power of two is translated into a cheaper shift operation:

.. code-block:: bash

		{
			"FamilyName": "IR",
			"Type": "StrengthReduction",
			"Name": "MulSigned32",
			"SubName": "ByPowerOfTwo",
			"Expression": "IR.MulSigned32 x c",
			"Filter": "IsResolvedConstant(c) & IsPowerOfTwo32(c) & !IsZero(c) & !IsOne(c)",
			"Result": "(IR.ShiftLeft32 x [GetPowerOfTwo(To32(c))])"
		}


Functions
=========

To find the available filter and expression functions, see the methods in the ``Filter Methods`` and ``Expression Methods`` regions of ``Source\Mosa.Compiler.Framework\Transform\BaseTransformation.cs``.

